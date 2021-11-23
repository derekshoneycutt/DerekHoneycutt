import { Imogene as $_ } from '../Imogene/Imogene';
import { MDCRipple } from '@material/ripple';
import { MDCTextField } from '@material/textfield';

/** Component for showing the contact form of my portfolio */
export default class DrockContact extends HTMLElement {
    #elementCache = {
        buttons: $_.makeEmpty(),
        textFields: $_.makeEmpty(),
        iconButtons: $_.makeEmpty(),

        backButton: $_.makeEmpty()
    };
    #mdcCache = {};

    constructor() {
        super();
    }

    connectedCallback() {
        // Only run this once from here
        if (this.shadowRoot)
            return;

        // Get the shadow root and template data
        const shadowRoot = $_.enhance(this.attachShadow({ mode: 'open' }));
        shadowRoot.appendChildren($_.find('#drock-contact').prop('content').cloneNode(true));

        this.#elementCache = {
            buttons: shadowRoot.find('.mdc-button'),
            textFields: shadowRoot.find('.mdc-text-field'),
            iconButtons: shadowRoot.find('.mdc-icon-button'),

            backButton: shadowRoot.find('.contact-topbar-back-button')
        };
        this.#mdcCache = {
            buttons: this.#elementCache.buttons.map(b => new MDCRipple(b)),
            textFields: this.#elementCache.textFields.map(tf => new MDCTextField(tf)),
            iconButtons: this.#elementCache.iconButtons.map(v => {
                v.mdcRipple = new MDCRipple(v);
                v.mdcRipple.unbounded = true;
            })
        };

        this.#elementCache.backButton.addEvents({
            click: e => this.onClose()
        });
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

    onClose = () => {
        this.dispatchEvent(new CustomEvent('requestclose', {
            bubbles: true,
            composed: true
        }));
    }

    onSend = () => {
        /*if (`${this._nameField.value}`.trim() === '') {
            return;
        }

        const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (!re.test(String(this._emailField.value).toLowerCase())) {
            return;
        }
        if (`${this._msgField.value}`.trim() === '') {
            return;
        }

        this.dispatchEvent(new CustomEvent('send', {
            detail: {
                from: this._nameField.value,
                return: this._emailField.value,
                message: this._msgField.value
            },
            bubbles: true,
            composed: true
        }));*/
    }

    clearFields = () => {
        /*this._emailField.value = '';
        this._nameField.value = '';
        this._msgField.value = '';*/
    }
}
window.customElements.define('drock-contact', DrockContact);