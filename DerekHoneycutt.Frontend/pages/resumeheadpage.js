import { Imogene as $_ } from '../Imogene/Imogene';

/** Component for resume head pages */
export default class DrockResumeHeadPage extends HTMLElement {
    #elementCache = {
        titleEl: $_.makeEmpty()
    };
    #values = {
        title: $_.value(this.title, v => v || '')
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
        shadowRoot.appendChildren($_.find('#drock-resumeheadpage').prop('content').cloneNode(true));

        this.#elementCache = {
            titleEl: shadowRoot.find('.drock-resumehead-title')
        };

        this.#elementCache.titleEl.emptyAndReplace(this.#values.title);
    }

    static get observedAttributes() {
        return ['title'];
    }

    /** The title that should be displayed on the page
     * @type {string} */
    get title() {
        return this.getAttribute('title') || '';
    }
    set title(value) {
        if (this.getAttribute('title') !== value)
            this.#setAttribute('title', value);
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
}
window.customElements.define('drock-resumeheadpage', DrockResumeHeadPage);