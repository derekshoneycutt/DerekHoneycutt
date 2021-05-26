import { Imogene as $, ImogeneExports as $_, ImogeneTemplate as $t } from '../../Imogene/Imogene';
import { MDCRipple } from '@material/ripple';
import { MDCTextField } from '@material/textfield';

/** Component for showing the contact form of my portfolio */
export default class DrockContact extends HTMLElement {
    constructor() {
        super();

        this._constructed = false;
    }

    connectedCallback() {
        if (this.shadowRoot)
            return;

        //If shadowroot is not already retrieved, create it, copy the template, and setup events & properties

        const shadowRoot = this.attachShadow({ mode: 'open' });
        /** @type {HTMLTemplateElement} */
        const template = $('#drock-contact')[0];
        const showChildren = template.content.cloneNode(true);

        const buttons = $(showChildren, '.mdc-button');
        buttons.forEach(v => v.mdcRipple = new MDCRipple(v));
        const textFields = $(showChildren, '.mdc-text-field');
        textFields.forEach(v => v.mdcTextField = new MDCTextField(v));
        const iconButtons = $(showChildren, '.mdc-icon-button');
        iconButtons.forEach(v => {
            v.mdcRipple = new MDCRipple(v);
            v.mdcRipple.unbounded = true;
        });

        this._backButton = $(showChildren, '.contact-topbar-back-button');
        this._backButton.addEvents({
            click: e => this.onClose()
        });
        /*this._nameField = $(showChildren, '.drock-contact-name-field')[0];
        this._emailField = $(showChildren, '.drock-contact-email-field')[0];
        this._msgField = $(showChildren, '.drock-contact-msg-field')[0];
        this._cancelButton = $(showChildren, '.drock-contact-cancel-btn');
        this._cancelButton.addEvents({
            click: e => this.onClose()
        });
        this._sendButton = $(showChildren, '.drock-contact-send-btn');
        this._sendButton.addEvents({
            click: e => this.onSend()
        });*/



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

    onClose() {
        this.dispatchEvent(new CustomEvent('requestclose', {
            bubbles: true,
            composed: true
        }));
    }

    onSend() {
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

    clearFields() {
        /*this._emailField.value = '';
        this._nameField.value = '';
        this._msgField.value = '';*/
    }
}
window.customElements.define('drock-contact', DrockContact);