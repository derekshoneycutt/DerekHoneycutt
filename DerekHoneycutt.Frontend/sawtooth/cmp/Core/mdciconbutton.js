import { Imogene as $, ImogeneExports as $_ } from '../../../Imogene/Imogene';
import { MDCRipple } from '@material/ripple';

/** Component for showing an MDC Icon Button */
export class MdcIconButton extends HTMLElement {
    constructor() {
        super();

        const shadowRoot = this.attachShadow({ mode: 'open' });
        /** @type {HTMLTemplateElement} */
        const template = $('#saw-mdc-icon-button')[0];
        const showChildren = template.content.cloneNode(true);

        const buttonEl = $(showChildren, '.saw-mdc-icon-button');
        buttonEl.forEach(b => {
            const iconButtonRipple = new MDCRipple(b);
            iconButtonRipple.unbounded = true;
        });
        /* * @type {HTMLSlotElement} */
        //const slotEl = $(showChildren, 'slot')[0];

        shadowRoot.appendChild(showChildren);
    }

    static get observedAttributes() {
        return [];
    }

    /*attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
        }
    }*/

    __setAttribute(attr, value) {
        if (value)
            this.setAttribute(attr, value);
        else if (this.hasAttribute(attr))
            this.removeAttribute(attr);
    }
}
window.customElements.define('saw-mdc-icon-button', MdcIconButton);