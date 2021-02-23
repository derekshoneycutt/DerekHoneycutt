import { Imogene as $, ImogeneExports as $_ } from '../Imogene/Imogene';

/** Splash screen component showing an image and disappearing */
export default class DrockSplashScreen extends HTMLElement {
    constructor() {
        super();

        this._src = $_.value(this.getAttribute('src'));

        const shadowRoot = this.attachShadow({ mode: 'open' });

        /** @type {HTMLTemplateElement} */
        const template = $('#drock-splashscreen')[0];
        const showChildren = template.content.cloneNode(true);
        $_.setProperties($(showChildren, '.drock-splashscreen-img'), {
            src: this._src
        });
        /** @type {HTMLDivElement} */
        this._shadowParent = $(showChildren, '.drock-splashscreen-parent');
        shadowRoot.appendChild(showChildren);
    }

    static get observedAttributes() {
        return [ 'src' ];
    }

    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case 'src':
                this._src.set(newValue);
                break;
        }
    }

    __setAttribute(attr, value) {
        if (value)
            this.setAttribute(attr, value);
        else if (this.hasAttribute(attr))
            this.removeAttribute(attr);
    }

    /** Image source to use for the splashscreen */
    get src() {
        return this.getAttribute('src');
    }
    set src(value) {
        this.__setAttribute('src', value);
    }

    /**
     * Pause for a time and fade the splashscreen
     * @param {number} timeout Time to pause before fading out
     * @param {number} fadeduration Duration to expect fading out
     */
    async pauseAndFade(timeout = 1000, fadeduration = 1000) {
        await new Promise(r => setTimeout(r, timeout));

        this._shadowParent[0].classList.add('hidden');

        await new Promise(r => setTimeout(r, fadeduration));

        this.remove();
    }
}
window.customElements.define('drock-splash', DrockSplashScreen);
