import { Imogene as $_ } from '../Imogene/Imogene';

/** Splash screen component showing an image and disappearing */
export default class DrockSplashScreen extends HTMLElement {
    #elementCache = {
        img: $_.makeEmpty(),
        shadowParent: $_.makeEmpty()
    };
    #values = {
        src: $_.value(this.getAttribute('src'))
    };

    constructor() {
        super();
    }

    connectedCallback() {
        // Only run this once from here
        if (this.shadowRoot)
            return;

        // Get the shadow root and template data
        const shadowRoot = $_.enhance(this.attachShadow({ mode: 'open' }));
        shadowRoot.appendChildren($_.find('#drock-splashscreen').prop('content').cloneNode(true));

        this.#elementCache = {
            img: shadowRoot.find('.drock-splashscreen-img'),
            shadowParent: shadowRoot.find('.drock-splashscreen-parent')
        };

        this.#elementCache.img.setProperties({
            src: this.#values.src
        });
    }

    static get observedAttributes() {
        return [ 'src' ];
    }

    attributeChangedCallback(name, oldValue, newValue) {
        const camelName = $_.camelize(name);
        if (this.#values[camelName]) {
            this.#values[camelName].set(newValue);
        }
    }

    #setAttribute = (attr, value) => {
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
        this.#setAttribute('src', value);
    }

    /**
     * Pause for a time and fade the splashscreen
     * @param {number} timeout Time to pause before fading out
     * @param {number} fadeduration Duration to expect fading out
     */
    async pauseAndFade(timeout = 1000, fadeduration = 1000) {
        await new Promise(r => setTimeout(r, timeout));

        this.#elementCache.shadowParent.addClass('hidden');

        await new Promise(r => setTimeout(r, fadeduration));

        this.remove();
    }
}
window.customElements.define('drock-splash', DrockSplashScreen);
