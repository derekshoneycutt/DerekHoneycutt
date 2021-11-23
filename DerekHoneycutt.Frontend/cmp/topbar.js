import { Imogene as $_ } from '../Imogene/Imogene';

/** Component for showing the topbar of my portfolio */
export default class DrockTopBar extends HTMLElement {
    constructor() {
        super();
    }

    connectedCallback() {
        // Only run this once from here
        if (this.shadowRoot)
            return;

        // Get the shadow root and template data
        const shadowRoot = $_.enhance(this.attachShadow({ mode: 'open' }));
        shadowRoot.appendChildren($_.find('#drock-topbar').prop('content').cloneNode(true));
    }

    static get observedAttributes() {
        return [];
    }

    /*attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
        }
    }*/

    #setAttribute = (attr, value) => {
        if (value)
            this.setAttribute(attr, value);
        else if (this.hasAttribute(attr))
            this.removeAttribute(attr);
    }
}
window.customElements.define('drock-topbar', DrockTopBar);