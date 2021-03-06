﻿import NotifyingValue from './NotifyingValue';
import { getOwnProperties, createTextNode } from './Base';

/** Used for handling a binding with a NotifyingValue (or just mapping to any element) in DOM */
export class DomBinding {
    /**
     * Construct a new binding
     * @param {any} value value to bind to
     * @param {HTMLElement} container container to bind the value to
     * @param {Array} [exist] Any existing elements to replace, if applicable
     */
    constructor(value, container, exist = null) {
        this.container = container;
        this.current = exist;
        this.value = value;
    }

    /** Hide any currently shown elements */
    hideAll() {
        hideOne = c => {
            if (c instanceof HTMLElement)
                c.remove();
            else if (c instanceof DomBinding)
                c.hideAll();
        };
        if (this.current && this.current instanceof Array)
            this.current.forEach(hideOne);
        else if (this.current)
            hideOne(this.current);
        this.current = null;
        if (this.value instanceof NotifyingValue)
            this.value.removeListener(this.replace);
    }

    /** Reduce the current rendered DOM down to a single element */
    reduceCurrent() {
        if (this.current && this.current instanceof Array) {
            this.current = this.current.reduce((p, v) => {
                if (p) {
                    if (v instanceof HTMLElement)
                        v.remove();
                    else if (v instanceof DomBinding)
                        v.hideAll();
                    return p;
                }
                if (v instanceof HTMLElement || v instanceof DomBinding)
                    return v;
                return null;
            }, null);
        }
        if (!this.current) return;

        let tempSpan = createTextNode('');
        if (this.current instanceof HTMLElement)
            this.current.replaceWith(tempSpan);
        else if (this.current instanceof DomBinding) {
            this.current.reduceCurrent();
            if (this.current.current)
                this.current.current.replaceWith(tempSpan);
            else
                this.container.appendChild(tempSpan);
            this.current.hideAll();
        }
        else
            this.container.appendChild(tempSpan);
        this.current = tempSpan;
    }

    /**
     * Get the replacement for a new value
     * @param {any} replaceWith New values to replace DOM with
     * @returns {{ doms: [], props: {}, binds: [], curr: [] }} Description of replacement for binding
     */
    getReplacement(replaceWith) {
        let newarr = [];
        if (replaceWith instanceof Array || replaceWith instanceof HTMLCollection ||
            replaceWith instanceof NodeList)
            newarr = [...replaceWith];
        else
            newarr = [replaceWith];

        let dowith = preprocChildren(...newarr)
            .reduce((p, v) => {
                if (v.toappend instanceof Node) {
                    p.doms.push(v.toappend);
                    p.curr.push(v.toappend);
                }
                else if (v.toappend instanceof NotifyingValue) {
                    let textdom = createTextNode('');
                    let bind = new DomBinding(v.toappend, this.container, textdom);
                    p.doms.push(textdom);
                    p.binds.push(bind);
                    p.curr.push(bind);
                }
                else if (v.toappend instanceof Object) {
                    Object.assign(p.props, v.toappend);
                }
                return p;
            }, { doms: [], props: {}, binds: [], curr: [] });

        return dowith;
    }

    /**
     * Replace the current rendered DOM with a new value
     * @param {any} replaceWith the new value to replace the DOM with
     * @returns {[]} The newly rendered DOM
     */
    replace(replaceWith) {
        this.reduceCurrent();

        let dowith = this.getReplacement(replaceWith);

        if (getOwnProperties(dowith.props).length > 0)
            setProperties(this.container, dowith.props);

        if (dowith.doms.length > 0) {
            if (this.current)
                this.current.replaceWith(...dowith.doms);
            else
                dowith.doms.forEach(dom => this.container.appendChild(dom));
            this.current = dowith.doms;
        }
        this.current = dowith.curr;

        dowith.binds.forEach(bind => bind.create());

        return this.current;
    }

    /** Create the DOM node and show it */
    create() {
        if (this.value instanceof NotifyingValue) {
            this.value.addListener(v => window.requestAnimationFrame(() => this.replace(v)));
            this.value.forceTrigger();
        }
        else
            this.replace(this.value);
    }
}

/**
 * Get the parent elements (may only be one if pass in a single node)
 * @param {Node | Node[] | NodeList | HTMLCollection} fromElement element to get parent of
 * @returns {Node[]} Array of parent elements
 */
export const parentElements = (fromElement) => {
    if (fromElement instanceof Node)
        return [fromElement.parentElement];
    else if (fromElement instanceof Array || fromElement instanceof HTMLCollection || fromElement instanceof NodeList)
        return [...fromElement].reduce((ret, el) => {
            ret.push(...parentElements(el));
            return ret;
        }, []);
    else
        return fromElement;
};

/**
 * Empty out an element(s)
 * @param {Node|Node[]|NodeList|HTMLCollection} el element to empty out
 */
export const empty = (el) => {
    if (el instanceof Node) {
        while (el.firstChild) {
            el.removeChild(el.firstChild);
        }
    }
    else if (el instanceof Array || el instanceof NodeList || el instanceof HTMLCollection) {
        [...el].forEach(e => empty(e));
    }
};

/**
 * Preprocess children elements to be appended to an element
 * @param {...any} children The child elements meant to be appended
 * @returns {{ child: any, toappend: any }[]} Preprocessed object describing what to append
 */
export const preprocChildren = (...children) =>
    children.reduce((p, child) => {
        if (!child) return p;
        let toappend = null;
        if (typeof child === 'string') {
            toappend = createTextNode(child);
        }
        else if (child instanceof Array) {
            toappend = makeNode(...child);
        }
        else if (child instanceof Node ||
                 child instanceof NotifyingValue ||
                 child instanceof Object) {
            toappend = child;
        }
        else {
            toappend = createTextNode(child);
        }

        p.push({
            child: child,
            toappend: toappend
        });

        return p;
    }, []);

/**
 * Append children to a node (if array, appends to the first Node)
 * @param {Node|Node[]|NodeList|HTMLCollection} el element to append to
 * @param {...any} children Children to append to the element
 */
export const appendChildren = (el, ...children) => {
    if (el instanceof Node) {
        preprocChildren(...children)
            .forEach(child => {
                if (child.toappend) {
                    if (child.toappend instanceof Node) {
                        el.appendChild(child.toappend);
                    }
                    else if (child.toappend instanceof NotifyingValue) {
                        (new DomBinding(child.toappend, el)).create();
                    }
                    else if (child.toappend instanceof Object) {
                        setProperties(el, child.toappend);
                    }
                }
            });
    }
    else if (el instanceof Array || el instanceof NodeList || el instanceof HTMLCollection) {
        /** @type {Node[]} */
        const nodes = [...el].filter(e => e instanceof Node);
        if (nodes.length > 0) {
            appendChildren(nodes[0], ...children);
        }
    }
};

/**
 * Empty the contents of an element and replace it with new children
 * @param {Node|Node[]|NodeList|HTMLCollection} el element to empty and replace
 * @param {...any} newvals New children to fill 
 */
export const emptyAndReplace = (el, ...newvals) => {
    empty(el);
    appendChildren(el, ...newvals);
};

/**
 * Add event(s) to the element(s)
 * @param {Node|Node[]|NodeList|HTMLCollection} el element(s) to add events to
 * @param {{}} events Events dictionary to add to the element(s)
 * @returns {Node|Node[]|NodeList|HTMLCollection} The element(s) modified
 */
export const addEvents = (el, events) => {
    if (el instanceof Node) {
        getOwnProperties(events)
            .forEach(event => el.addEventListener(event, events[event]));
    }
    else if (el instanceof Array || el instanceof NodeList || el instanceof HTMLCollection) {
        [...el].forEach(elm => addEvents(elm, events));
    }
    return el;
};

/**
 * Add event(s) to the element(s)
 * @param {Node|Node[]|NodeList|HTMLCollection} el element(s) to add events to
 * @param {{}} events Events dictionary to add to the element(s)
 * @returns {Node|Node[]|NodeList|HTMLCollection} The element(s) modified
 */
export const removeEvents = (el, events) => {
    if (el instanceof Node) {
        getOwnProperties(events)
            .forEach(event => el.removeEventListener(event, events[event]));
    }
    else if (el instanceof Array || el instanceof NodeList || el instanceof HTMLCollection) {
        [...el].forEach(elm => removeEvents(elm, events));
    }
    return el;
};

/**
 * Set a set of CSS class lists to element(s)
 * @param {Node|Node[]|NodeList|HTMLCollection} el element to set the class list for
 * @param {{}} classList Dictionary of classes to set or unset
 * @returns {Node|Node[]|NodeList|HTMLCollection} Modified node(s)
 */
export const setClassList = (el, classList) => {
    if (el instanceof Array || el instanceof NodeList || el instanceof HTMLCollection) {
        [...el].forEach(elm => setClassList(elm, classList));
    }
    else {
        getOwnProperties(classList)
            .forEach(key => {
                const doset = b => {
                    if (b) el.classList.add(key);
                    else el.classList.remove(key);
                };
                let setvalue = classList[key];
                if (setvalue instanceof NotifyingValue) {
                    setvalue.addListener(v => window.requestAnimationFrame(() => doset(!!v)));
                    setvalue.forceTrigger();
                }
                else {
                    doset(!!setvalue);
                }
            });
    }
    return el;
};

/**
 * Add CSS class(es) to node(s)
 * @param {Node|Node[]|NodeList|HTMLCollection} el element to add classes to
 * @param {string} className class names to add
 * @returns {Node|Node[]|NodeList|HTMLCollection} Modified node(s)
 */
export const addClass = (el, className) => {
    if (el instanceof Array || el instanceof NodeList || el instanceof HTMLCollection) {
        el.forEach(elm => addClass(elm, className));
    }
    else {
        className.split(' ')
            .filter(c => c && c !== '')
            .forEach(c => el.classList.add(c));
    }
    return el;
};

/**
 * Set CSS style values
 * @param {Node|Node[]|NodeList|HTMLCollection} el element(s) to modify style on
 * @param {{}} styleObj Dictionary of CSS styles to modify
 * @returns {{}|{}[]} Previous style values that were changed
 */
export const setStyle = (el, styleObj) => {
    let ret = {};
    if (el instanceof Array || el instanceof NodeList || el instanceof HTMLCollection) {
        ret = [...el].map(elm => setStyle(elm, styleObj));
    }
    else {
        ret = getOwnProperties(styleObj)
            .reduce((ret, style) => {
                ret[style] = el.style.getPropertyValue(style);
                const currStyle = styleObj[style];
                if (currStyle instanceof NotifyingValue) {
                    currStyle.addListener(v => window.requestAnimationFrame(() => el.style.setProperty(style, v)));
                    currStyle.forceTrigger();
                }
                else {
                    el.style.setProperty(style, styleObj[style]);
                }
                return ret;
            }, {});
    }
    return ret;
};

/**
 * Set a property on a node
 * @param {HTMLElement} el element to modify
 * @param {string} prop name of the property to modify
 * @param {NotifyingValue|string} value value of the property to set
 */
const setProperty = (el, prop, value) => {
    if (typeof value === 'undefined') {
        el.removeAttribute(prop);
    }
    else if (value instanceof NotifyingValue) {
        value.addListener(v => window.requestAnimationFrame(() => setProperty(el, prop, v)));
        value.forceTrigger();
    }
    else if (prop === 'on') {
        addEvents(el, value);
    }
    else if (prop === 'class') {
        addClass(el, value);
    }
    else if (prop === 'classList') {
        setClassList(el, value);
    }
    else if (prop === 'style') {
        if (typeof value === 'string')
            el.setAttribute(prop, value);
        else
            setStyle(el, value);
    }
    else if (prop === 'innerHTML') {
        el.innerHTML = value;
    }
    else {
        el.setAttribute(prop, value);
    }
};

/**
 * Set properties on a set of elements
 * @param {HTMLElement|HTMLElement[]|HTMLCollection} el element(s) to modify
 * @param {{}} props Dictionary of properties to modify
 * @returns {HTMLElment|HTMLElement[]|HTMLCollection} element(s) that have been modified
 */
export const setProperties = (el, props) => {
    if (el instanceof Array || el instanceof NodeList || el instanceof HTMLCollection) {
        [...el].forEach(elm => setProperties(elm, props));
    }
    else {
        getOwnProperties(props)
            .forEach(key => {
                let value = props[key];
                setProperty(el, key, value);
            });
    }
    return el;
};

/**
 * Make a new DOM node
 * @param {string} name The name of the type of element to create
 * @param {...any} children The children to add to the new element
 * @returns {HTMLElment} A newly created element
 */
export const makeNode = (name, ...children) => {
    let newNode = name instanceof Node ? name : document.createElement(name);
    appendChildren(newNode, ...children);
    return newNode;
};
