import { Imogene as $_ } from '../../Imogene/Imogene';
import { MDCTabBar } from '@material/tab-bar';
import { MDCRipple } from '@material/ripple';

/** @typedef {Object} DrockTopbarTabDef
  * @property {string} icon Icon to use
  * @property {string} label Label to put on the tab
  * @property {boolean} [active] Whether this is an active tab or not */

/** Component for showing the topbar of my portfolio */
export default class DrockTopBar extends HTMLElement {
    #elementCache = {
        tabBarContent: $_.makeEmpty(),
        tabBar: $_.makeEmpty()
    };
    #constructed = false;

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

        this.#elementCache = {
            tabBarContent: shadowRoot.find('.mdc-tab-scroller__scroll-content'),
            tabBar: shadowRoot.find('.mdc-tab-bar')
        };
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

    /**
     * Construct a tab based on an icon and a label
     * @param {string} icon Icon to use
     * @param {string} label Label to put on the tab
     * @param {boolean} [active] Whether this is an active tab or not
     */
    #constructTab = (icon, label, active = false) => {
        return $_.make('button', {
            class: 'mdc-tab mdc-tab--stacked ' + (active ? 'mdc-tab--active' : ''),
            role: 'tab',
            "aria-selected": active,
            tabindex: 0
        },
            ['span', { class: 'mdc-tab__content' },
                ['span', { class: 'mdc-tab__icon material-icons', 'aria-hidden': true }, icon],
                ['span', { class: 'mdc-tab__text-label' }, label]
            ],
            ['span', { class: 'mdc-tab-indicator ' + (active ? 'mdc-tab-indicator--active' : '') },
                ['span', { class: 'mdc-tab-indicator__content mdc-tab-indicator__content--underline' }]
            ],
            ['span', { class: 'mdc-tab__ripple' }]
        );
    }

    /**
     * Event triggered when the tabbar changes
     * @param {any} event
     */
    onTabbarActivate = (event) => {
        this.dispatchEvent(new CustomEvent('tabbarchange', {
            detail: {
                index: event.detail.index
            },
            bubbles: true,
            composed: true
        }));
    }

    /**
     * 
     * @param {DrockTopbarTabDef[]} tabs Tabs fill the tab-bar with
     */
    fillTabs = (tabs) => {
        if (this.#constructed)
            return;

        var madeTabs = tabs.map(t => this.#constructTab(t.icon, t.label, t.active));
        this.#elementCache.tabBarContent.appendChildren(...madeTabs);

        this.#elementCache.tabBar.forEach(tb => {
            let mdcTabBar = new MDCTabBar(tb);
            mdcTabBar.listen("MDCTabBar:activated", e => {
                this.onTabbarActivate(e);
            });
            tb.mdcTabBar = mdcTabBar;
        });

        this.#constructed = true;
    }

    /**
     * Move the tabbar to the given index
     * @param {number} index Index of the tab to move the TabBar to
     */
    moveToTabIndex = (index) => {
        this.#elementCache.tabBar.forEach(tb => {
            tb.mdcTabBar.activateTab(index);
        });
    }
}
window.customElements.define('drock-topbar', DrockTopBar);