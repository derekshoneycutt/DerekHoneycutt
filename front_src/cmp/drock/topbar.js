import { Imogene as $, ImogeneExports as $_, ImogeneTemplate as $t } from '../../Imogene/Imogene';
import { MDCTabBar } from '@material/tab-bar';
import { MDCRipple } from '@material/ripple';

/** @typedef {Object} DrockTopbarTabDef
  * @property {string} icon Icon to use
  * @property {string} label Label to put on the tab
  * @property {boolean} [active] Whether this is an active tab or not */

/** Component for showing the topbar of my portfolio */
export class DrockTopBar extends HTMLElement {
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
        const template = $('#drock-topbar')[0];
        const showChildren = template.content.cloneNode(true);

        this._tabBarContent = $(showChildren, '.mdc-tab-scroller__scroll-content');
        this._tabBar = $(showChildren, '.mdc-tab-bar');

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

    /**
     * Construct a tab based on an icon and a label
     * @param {string} icon Icon to use
     * @param {string} label Label to put on the tab
     * @param {boolean} [active] Whether this is an active tab or not
     */
    _constructTab(icon, label, active = false) {
        return $(['button', {
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
        ]);
    }

    onTabbarActivate(event) {
        //if (event.detail.index === 5)
        //    window.location.href = "https://subaruvagabond.com/";
        console.log(`Activated: ${event.detail.index}`);
    }

    /**
     * 
     * @param {DrockTopbarTabDef[]} tabs Tabs fill the tab-bar with
     */
    fillTabs(tabs) {
        if (this._constructed)
            return;

        var madeTabs = tabs.map(t => this._constructTab(t.icon, t.label, t.active));
        this._tabBarContent.appendChildren(...madeTabs);

        this._tabBar.forEach(tb => {
            let mdcTabBar = new MDCTabBar(tb);
            mdcTabBar.listen("MDCTabBar:activated", e => {
                this.onTabbarActivate(e);
            });
            tb.mdcTabBar = mdcTabBar;
        });

        this._constructed = true;
    }
}
window.customElements.define('drock-topbar', DrockTopBar);