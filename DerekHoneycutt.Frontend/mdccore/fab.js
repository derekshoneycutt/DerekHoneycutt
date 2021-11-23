import { Imogene as $_ } from '../Imogene/Imogene';
import { MDCRipple } from '@material/ripple';

/** Component for showing a MDC-based Floating Action Button */
export default class DrockFab extends HTMLElement {
    #elementCache = {
        fab: $_.makeEmpty(),
        iconEl: $_.makeEmpty()
    };
    #values = {
        icon: $_.value('add', v => v || 'add')
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
        shadowRoot.appendChildren($_.find('#drock-fab').prop('content').cloneNode(true));

        this.#elementCache = {
            fab: shadowRoot.find('.mdc-fab'),
            iconEl: shadowRoot.find('.mdc-fab__icon')
        };

        this.#elementCache.fab.forEach(f => {
            const fabRipple = new MDCRipple(f);
            f.mdcRipple = fabRipple;
        });
        this.#elementCache.iconEl.emptyAndReplace(this.#values.icon);
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
            this.#setAttribute('icon', value);
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
window.customElements.define('drock-fab', DrockFab);