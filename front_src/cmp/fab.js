﻿import { Imogene as $, ImogeneExports as $_ } from '../Imogene/Imogene';
import { MDCRipple } from '@material/ripple';

/** Component for showing a MDC-based Floating Action Button */
export class DrockFab extends HTMLElement {
    constructor() {
        super();

        const shadowRoot = this.attachShadow({ mode: 'open' });
        /** @type {HTMLTemplateElement} */
        const template = $('#drock-fab')[0];
        const showChildren = template.content.cloneNode(true);

        /** @type {HTMLButtonElement[]} */
        const fab = $(showChildren, '.mdc-fab');
        fab.forEach(f => {
            const fabRipple = new MDCRipple(f);
            f.mdcRipple = fabRipple;
        });
        /** @type {HTMLSpanElement} */
        this.__iconEl = $(showChildren, '.mdc-fab__icon')[0];
        this.__iconEl.innerHTML = this.icon;
        /* * @type {HTMLSlotElement} */
        //const slotEl = $(showChildren, 'slot')[0];

        shadowRoot.appendChild(showChildren);
    }

    static get observedAttributes() {
        return ['icon'];
    }

    /** The icon that should be displayed on the FAB
     * @type {string} */
    get icon() {
        return this.getAttribute('icon') || 'add';
    }
    set icon(value) {
        if (this.getAttribute('icon') !== value)
            this.__setAttribute('icon', value);
        this.__iconEl.innerHTML = value || 'add';
    }

    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case 'icon':
                this.icon = newValue;
                break;
            default:
                break;
        }
    }

    __setAttribute(attr, value) {
        if (value)
            this.setAttribute(attr, value);
        else if (this.hasAttribute(attr))
            this.removeAttribute(attr);
    }
}
window.customElements.define('drock-fab', DrockFab);