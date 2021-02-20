import { Imogene as $, ImogeneExports as $_ } from '../Imogene/Imogene';
import { MDCRipple } from '@material/ripple';

/** Offset pixels to break into swiping actions */
const barrierOffset = 50;

/** Element that handles swiping */
export default class DrockSwiperElement extends HTMLElement {
    constructor() {
        super();

        /** How much larger than the actual hover icons should the hover be activated by */
        this.hoverMultiplier = 1.5;
        /** Whether swiping is currently allowed within the compononet */
        this.allowSwipe = true;
        /**  Whether hover navigation should be allowed to be shown in the component */
        this.allowPopoverNav = true;

        /** Whether the component is currently in a swipe operation */
        this.isSwiping = false;

        /** Current index being displayed */
        this.__currIndex = 0;
        /** Number of children recognized in the swipe area */
        this.__childCount = 0;
        /** Last position of the mouse in a mouse event */
        this.__lastMouse = 0;

        /** Contains the starting point of user's touch that initiated possible swiping */
        this.start = null;
        /** Contains measurements taken at start of swiping action, used for maths */
        this.startMod = {
            x: 0,
            width: 0,
            y: 0,
            height: 0
        };
        /** Contains bounds & pivot information for movement inside of a swipe */
        this.minmax = {
            min: 0,
            max: 0,
            pivot: 0,
            subpivot: 0
        };
        /** Marks if crossed a boundary allowing for swiping actions */
        this.crossedBoundary = false;
        /** Marks if user appears to have begun scrolling in opposing axis, blocking swiping actions */
        this.beganScroll = false;

        /** Marks whether currently scrolling with the mouse wheel */
        this.isWheeling = false;
        /** Current position of scrolling with the mouse wheel */
        this.wheelPosition = 0;
        /** Current timeout to handle when mouse wheeling has ended */
        this.wheelTimeout = null;
    }

    connectedCallback() {
        if (this.shadowRoot)
            return;

        //If shadowroot is not already retrieved, create it, copy the template, and setup events & properties

        const shadowRoot = this.attachShadow({ mode: 'open' });

        /** @type {HTMLTemplateElement} */
        const template = $('#drock-swiper')[0];
        const showChildren = template.content.cloneNode(true);

        /** Link to move previous
         *  @type {HTMLAnchorElement} */
        this.__prevlink = $(showChildren, '.drock-swiper-prev');
        this.__prevlink.forEach(f => {
            const fabRipple = new MDCRipple(f);
            f.mdcRipple = fabRipple;
        });
        this.__prevlink.addEvents({
            click: e => {
                this.movePrevious();
                e.preventDefault();
            }
        });
        this.__prevlink.setClassList({
            'drock-swiper-hidden': this.hidexmove
        });

        this.__uplink = $(showChildren, '.drock-swiper-upprev');
        this.__uplink.forEach(b => {
            b.mdcRipple = new MDCRipple(b);
        });
        this.__uplink.addEvents({
            click: e => {
                this.movePrevious();
                e.preventDefault();
            }
        });
        this.__uplink.setClassList({
            'drock-swiper-hidden': true
        });

        /** Link to move next
         * @type {HTMLAnchorElement} */
        this.__nextlink = $(showChildren, '.drock-swiper-next');
        this.__nextlink.forEach(f => {
            const fabRipple = new MDCRipple(f);
            f.mdcRipple = fabRipple;
        });
        this.__nextlink.addEvents({
            click: e => {
                this.moveNext(e);
                e.preventDefault();
            }
        });
        this.__nextlink.setClassList({
            'drock-swiper-hidden': this.hidexmove
        });

        this.__downlink = $(showChildren, '.drock-swiper-downnext');
        this.__downlink.forEach(b => {
            b.mdcRipple = new MDCRipple(b);
        });
        this.__downlink.addEvents({
            click: e => {
                this.moveNext(e);
                e.preventDefault();
            }
        });
        this.__downlink.setClassList({
            'drock-swiper-hidden': true
        });

        /** @type {HTMLDivElement} */
        const container = $(showChildren, '.drock-swiper-container');
        container.addEvents({
            mousemove: e => {
                this.__lastMouse = e[`client${this.orientation.toUpperCase()}`];
                this.updateNavShown();
            },
            mouseleave: e => {
                this.__lastMouse = undefined;
                this.updateNavShown();
            }
        });
        this.style.setProperty("--drock-swiper-orientation",
            this.orientation === 'x' ? 'row' : 'column');

        container[0].addEventListener('touchstart', e => this.onSwipeBegin(e), true);
        container[0].addEventListener('touchmove', e => this.onSwipeMove(e), true);
        container[0].addEventListener('touchend', e => this.onSwipeEnd(e, true), true);
        container[0].addEventListener('touchcancel', e => this.onSwipeEnd(e, false), true);
        container[0].addEventListener('wheel', e => this.onWheel(e));

        /** @type {HTMLSlotElement} */
        const slot = $(container, 'slot');
        slot.addEvents({
            slotchange: e => {
                const nodes = slot[0].assignedNodes();
                this.__childCount = nodes.length;
                this.updateNavShown();
            }
        });

        shadowRoot.appendChild(showChildren);
    }

    get index() {
        return this.__currIndex;
    }
    set index(value) {
        this.__setAttribute('index', value);
    }

    get hidexmove() {
        return !!this.getAttribute('hidexmove') || false;
    }
    set hidexmove(value) {
        this.__setAttribute('hidexmove', !!value);
    }

    get hideymove() {
        return !!this.getAttribute('hideymove') || false;
    }
    set hideymove(value) {
        this.__setAttribute('hideymove', !!value);
    }

    /**
     * Describes the current orientation of the swiper (x vs y)
     * @type {String} 
     */
    get orientation() {
        let ret = this.getAttribute("orientation") || "";
        if (ret.toLowerCase().substr(0, 1) === 'y')
            return 'y';
        return 'x';
    }
    set orientation(value) {
        if ((value || "").toLowerCase().substr(0, 1) === 'y')
            this.__setAttribute('orientation', 'y');
        else
            this.__setAttribute('orientation', 'x');
    }

    /** Whether to allow overshooting a swipe and going into the next space
     * @type {Boolean} */
    get allowOvershoot() {
        return !!this.getAttribute("allowovershoot");
    }
    set allowOvershoot(value) {
        this.__setAttribute("allowovershoot", !!value);
    }

    static get observedAttributes() {
        return ["index", "hidexmove", "hideymove", "orientation", "allowovershoot"];
    }

    attributeChangedCallback(name, oldValue, newValue) {
        /*const camelName = $_.camelize(name);
        if (this._values[camelName]) {
            this._values[camelName].set(newValue);
        }*/
        if (name.toLowerCase() === 'index') {
            let index = Math.trunc(parseInt(this.index, 0));
            if (index !== this.__currIndex)
                this.moveToIndex(index);
        }
        else if (name.toLowerCase() === 'hidexmove') {
            if (!this.__prevlink || !this.__nextlink)
                return;
            this.__prevlink.setClassList({
                'drock-swiper-hidden': this.hidexmove
            });
            this.__nextlink.setClassList({
                'drock-swiper-hidden': this.hidexmove
            });
        }
        else if (name.toLowerCase() === 'hideymove') {
            if (!this.__uplink || !this.__downlink)
                return;
            this.__uplink.setClassList({
                'drock-swiper-hidden': this.hideymove
            });
            this.__downlink.setClassList({
                'drock-swiper-hidden': this.hideymove
            });
        }
        else if (name.toLowerCase() === 'orientation') {
            this.style.setProperty("--drock-swiper-orientation",
                this.orientation === 'x' ? 'row' : 'column');
        }
    }

    __setAttribute(attr, value) {
        if (value)
            this.setAttribute(attr, value);
        else if (this.hasAttribute(attr))
            this.removeAttribute(attr);
    }


    /**
     * Get the current point of the cursor from an event
     * @param {MouseEvent} evt event to get information from
     * @returns {{x : number, y : number}} current point of the cursor
     */
    getGesturePointFromEvent(evt) {
        var point = {
            x: 0, y: 0
        };

        if (evt.changedTouches && evt.changedTouches[0]) {
            point.x = evt.changedTouches[0].clientX;
            point.y = evt.changedTouches[0].clientY;
        } else {
            point.x = evt.clientX;
            point.y = evt.clientY;
        }

        return point;
    }

    /**
     * Updates the internal minmax for swiping actions
     * @param {number} value value to update the minmax value according to
     */
    updateminmax(value) {
        //First, update min/max if we've crossed them. Tells us where we've been
        if (value < this.minmax.min)
            this.minmax.min = value;
        if (value > this.minmax.max)
            this.minmax.max = value;

        //PIVOT: Means can start swiping R and then change to swipe L and actually complete it!
            //It's like a preview!
        //Pivot and subpivot circle each other to determine swiping direction
            //Only consider pivoting if passed the standard barrier length away from the present subpivot
        let subpivDiff = value - this.minmax.subpivot;
        if (Math.abs(subpivDiff) > barrierOffset) {
            let pivDiff = this.minmax.pivot - this.minmax.subpivot;
            //If still headed in same direction, update the subpivot to new location, creating new barrier to pivot
            if (Math.sign(subpivDiff) === Math.sign(pivDiff))
                this.minmax.subpivot = value;
            else {
                //If sufficiently changed direction across the pivot, pivot directions!
                    //This needs special min/max resets as well to reset the new swipe-space
                this.minmax.pivot = this.minmax.subpivot;
                this.minmax.subpivot = value;
                if (subpivDiff > 0) {
                    this.minmax.min = this.minmax.pivot;
                    this.minmax.max = value;
                } else {
                    this.minmax.max = this.minmax.pivot;
                    this.minmax.min = value;
                }
            }
        }
    }

    /**
     * Event to handle when swiping begins
     * @param {MouseEvent} e event object for beginning swiping
     */
    onSwipeBegin(e) {
        if (!this.allowSwipe)
            return;

        //Note: This doesn't *actually* initiate swiping, though it does prepare the component expecting movement
        const curroffset = this.getBoundingClientRect();
        const detailPoint = this.getGesturePointFromEvent(e);
        this.start = detailPoint;
        this.startMod.width = curroffset.width;
        this.startMod.height = curroffset.height;
        this.startMod.x = -1 * curroffset.width * this.__currIndex;
        this.startMod.y = -1 * curroffset.height * this.__currIndex;
        this.isSwiping = true;
        this.minmax = {
            min: detailPoint[this.orientation],
            max: detailPoint[this.orientation],
            pivot: detailPoint[this.orientation],
            subpivot: detailPoint[this.orientation] + 1 //because fun (Start out as if heading R)
        };
        this.crossedBoundary = false;
        this.beganScroll = false;
        this.allowPopoverNav = false;
        this.updateNavShown();
        this.style.setProperty('--drock-swiper-slottranslength', '0');
    }

    /**
     * Event for the moues moving during a swiping event
     * @param {MouseEvent} evt event object to process
     */
    onSwipeMove(evt) {
        const antiorient = this.orientation === 'x' ? 'y' : 'x';
        const dimension = this.orientation === 'x' ? 'width' : 'height';

        if (this.isSwiping && !this.beganScroll) {
            const detailPoint = this.getGesturePointFromEvent(evt);
            this.updateminmax(detailPoint[this.orientation]);

            //If not crossed previously, test if we have now and only continue if so
            if (!this.crossedBoundary) {
                //If sufficiently crossed the standard barrier in the anti position only, block swiping!
                if (Math.abs(detailPoint[antiorient] - this.start[antiorient]) > barrierOffset) {
                    this.beganScroll = true;
                    return;
                }
                if (Math.abs(detailPoint[this.orientation] - this.start[this.orientation]) > barrierOffset)
                    this.crossedBoundary = true;
                else return;
            }
            evt.cancelable && evt.preventDefault();

            //Move the children, refusing to go beyond the first and last, if appropriate
            let use = this.startMod[this.orientation] + detailPoint[this.orientation] - this.start[this.orientation];
            if (!this.allowOvershoot) {
                if (use > 0)
                    use = 0;
                if (use < (this.__childCount - 1) * this.startMod[dimension] * -1)
                    use = (this.__childCount - 1) * this.startMod[dimension] * -1;
            }

            this.style.setProperty(`--drock-swiper-positionoffset${this.orientation.toUpperCase()}`, `${use}px`);
        }
    }

    /**
     * Event to run to finish a swiping action
     * @param {MouseEvent} evt event object representing the swipe finish event
     * @param {Boolean} didend Whether this is a true finish to the swiping
     */
    onSwipeEnd(evt, didend) {
        if (this.isSwiping) {
            this.isSwiping = false;

            if (evt.cancelable && didend && this.crossedBoundary)
                evt.preventDefault();

            this.style.setProperty('--drock-swiper-slottranslength', 'var(--drock-transitions-length, 0.3s)');

            //Although difference from start is useful, the pivot allows us some more dynamic use
                //Pivot resets minmaxX object, allowing us to use min/max to figure out swipe direction
            const detailPoint = this.getGesturePointFromEvent(evt);
            let diff = detailPoint[this.orientation] - this.minmax.pivot;
            if (diff > 0)
                diff = detailPoint[this.orientation] - this.minmax.min;
            else
                diff = detailPoint[this.orientation] - this.minmax.max;

            let moveTo = this.__currIndex;
            if (!this.beganScroll && Math.abs(diff) > barrierOffset) {
                //Note: You do need to cross back over your starting point to get a full different swipe
                    // Difference between going back to starting child or moving in opposite direction entirely
                let diffFromStart = detailPoint[this.orientation] - this.start[this.orientation];
                if (diff < 0 && diffFromStart <= 0)
                    ++moveTo;
                else if (diff > 0 && diffFromStart >= 0)
                    --moveTo;
            }
            this.moveToIndex(moveTo);

            this.start = null;
            this.allowPopoverNav = true;
            this.updateNavShown();
        }
    }

    /**
     * Event raised when the mouse wheel is used on the component
     * @param {WheelEvent} evt event object sent with the event
     */
    onWheel(evt) {
        const moveNumber = evt[`delta${this.orientation.toUpperCase()}`];
        if (Math.abs(moveNumber) < 25 && !this.isWheeling || this.__childCount <= 1)
            return;

        evt.preventDefault();

        if (this.isWheeling) {
            clearTimeout(this.wheelTimeout);
            this.wheelPosition -= moveNumber;
        }
        else {
            this.isWheeling = true;
            this.wheelPosition = -1 * moveNumber;

            const curroffset = this.getBoundingClientRect();
            this.startMod.width = curroffset.width;
            this.startMod.height = curroffset.height;
            this.startMod.x = -1 * curroffset.width * this.__currIndex;
            this.startMod.y = -1 * curroffset.height * this.__currIndex;

            this.style.setProperty('--drock-swiper-slottranslength', '0');
        }

        const dimension = this.orientation === 'x' ? 'width' : 'height';
        let use = this.startMod[this.orientation] + this.wheelPosition;
        if (use < this.startMod[this.orientation] - this.startMod[dimension])
            use = this.startMod[this.orientation] - this.startMod[dimension];
        if (use > this.startMod[this.orientation] + this.startMod[dimension])
            use = this.startMod[this.orientation] + this.startMod[dimension];
        if (!this.allowOvershoot) {
            if (use > 0)
                use = 0;
            if (use < (this.__childCount - 1) * this.startMod[dimension] * -1)
                use = (this.__childCount - 1) * this.startMod[dimension] * -1;
        }
        this.style.setProperty(`--drock-swiper-positionoffset${this.orientation.toUpperCase()}`, `${use}px`);
        this.wheelTimeout = setTimeout(() => {
            clearTimeout(this.wheelTimeout);
            this.wheelTimeout = null;
            this.isWheeling = false;
            this.style.setProperty('--drock-swiper-slottranslength', 'var(--drock-transitions-length, 0.3s)');

            let dir = -1 * Math.sign(this.wheelPosition);
            let doMove = this.__currIndex;
            if (Math.abs(this.wheelPosition) > 50) {
                doMove = this.__currIndex + dir;
            }
            this.wheelPosition = 0;
            this.moveToIndex(doMove, true);
        }, 150);
    }

    /** Update whether navigation buttons are shown overlaying the viewport */
    updateNavShown() {
        if (this.hideymove) {
            this.__uplink.setClassList({
                'drock-swiper-hidden': true
            });
            this.__downlink.setClassList({
                'drock-swiper-hidden': true
            });
        }
        else {
            this.__uplink.setClassList({
                'drock-swiper-hidden': (this.__currIndex < 1)
            });
            this.__downlink.setClassList({
                'drock-swiper-hidden': (this.__currIndex >= this.__childCount - 1)
            });
        }

        if (!this.allowPopoverNav || this.__lastMouse === undefined) {
            this.__prevlink[0].classList.toggle('drock-swiper-shownav', false);
            this.__nextlink[0].classList.toggle('drock-swiper-shownav', false);
        }
        else {
            let curroffset = this.getBoundingClientRect();
            const prevlinkoffset = this.__prevlink[0].getBoundingClientRect();

            const prevEnough = this.orientation === 'x' ?
                curroffset.left + prevlinkoffset.width * this.hoverMultiplier :
                curroffset.top + prevlinkoffset.height * this.hoverMultiplier;
            const nextEnough = this.orientation === 'x' ?
                curroffset.left + curroffset.width - prevlinkoffset.width * this.hoverMultiplier :
                curroffset.top + curroffset.height - prevlinkoffset.height * this.hoverMultiplier;

            this.__prevlink[0].classList.toggle('drock-swiper-shownav',
                this.allowPopoverNav && this.__currIndex > 0 &&
                this.__lastMouse <= prevEnough);
            this.__nextlink[0].classList.toggle('drock-swiper-shownav',
                this.allowPopoverNav && this.__currIndex < this.__childCount - 1 &&
                this.__lastMouse >= nextEnough);
        }
    }

    /**
     * Move to a given index of items within the swiping viewport
     * @param {number} index index to move to
     * @param {boolean} animate whether to animate the movement
     */
    moveToIndex(index, animate = true) {
        let useIndex = index;
        if (index >= this.__childCount)
            useIndex = this.__childCount - 1;
        if (index < 0)
            useIndex = 0;

        if (!animate)
            this.style.setProperty('--drock-swiper-slottranslength', '0');

        this.__currIndex = useIndex;
        this.style.setProperty(`--drock-swiper-positionoffset${this.orientation.toUpperCase()}`, `-${useIndex}00%`);

        if (!animate)
            this.style.setProperty('--drock-swiper-slottranslength', 'var(--drock-transitions-length, 0.3s)');
        this.updateNavShown();

        this.index = this.__currIndex;

        this.dispatchEvent(new CustomEvent('swipemove', {
            detail: {
                index: this.__currIndex
            },
            bubbles: true,
            composed: true
        }));
    }

    /** Move to the previous item in the viewport */
    movePrevious() {
        this.moveToIndex(this.__currIndex - 1);
    }

    /** Move to the next item in the viewport */
    moveNext() {
        this.moveToIndex(this.__currIndex + 1);
    }
}
window.customElements.define('drock-swiper', DrockSwiperElement);
