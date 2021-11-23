import { Imogene as $_ } from '../Imogene/Imogene';
import { MDCTabBar } from '@material/tab-bar';
import { MDCRipple } from '@material/ripple';

// Tab bar item can be a super simple, easy thing! :D
export class DrockTabBarItem extends HTMLElement {
    constructor() {
        super();
    }

    static get observedAttributes() {
        return ['icon', 'title', 'active'];
    }

    attributeChangedCallback(name, oldValue, newValue) {
        
    }

    #setAttribute = (attr, value) => {
        if (value)
            this.setAttribute(attr, value);
        else if (this.hasAttribute(attr))
            this.removeAttribute(attr);
    }

    /** The icon to show on the item */
    get icon() {
        return this.getAttribute('icon') || '';
    }
    set icon(value) {
        this.#setAttribute('icon', value);
    }

    /** The title to show on the item */
    get title() {
        return this.getAttribute('title') || '';
    }
    set title(value) {
        this.#setAttribute('title', value);
    }

    /** Whether to show the item in the active state */
    get active() {
        if (this.hasAttribute('active'))
            return !!this.getAttribute('active');
        return false;
    }
    set active(value) {
        this.#setAttribute('active', !!value);
    }
}
window.customElements.define('drock-mdctabbaritem', DrockTabBarItem);

/** Component for showing a tabbar */
export default class DrockTabBar extends HTMLElement {
    #elementCache = {
        tabBarContent: $_.makeEmpty(),
        tabBar: $_.makeEmpty(),
        slot: $_.makeEmpty(),
        slotchildren: $_.makeEmpty()
    };
    #itemdefs = [];
    #currIndex = 0;

    constructor() {
        super();
    }

    connectedCallback() {
        // Only run this once from here
        if (this.shadowRoot)
            return;

        // Get the shadow root and template data
        const shadowRoot = $_.enhance(this.attachShadow({ mode: 'open' }));
        shadowRoot.appendChildren($_.find('#drock-mdctabbar').prop('content').cloneNode(true));

        this.#elementCache = {
            tabBarContent: shadowRoot.find('.mdc-tab-scroller__scroll-content'),
            tabBar: shadowRoot.find('.mdc-tab-bar'),
            slot: shadowRoot.find('slot'),

            slotchildren: []
        };

        // Setup the tabbar slots!
        this.#setupAllSlots();

        // Listen to the slot changes
        this.#elementCache.slot.addEvents({
            slotchange: e => {
                this.#setupAllSlots();
            }
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

    /**
     * Callback raised when a slot item is changed (via MutationObserver)
     * @param {MutationRecord[]} details array of mutations made
     * @param {MutationObserver} observer observer calling the method
     */
    #onSlotItemChanged = (details, observer) => {
        this.#itemdefs
            .filter(itemdef => itemdef.mutationObserver === observer)
            .forEach((itemdef, index) => {
                details.forEach(record => {
                    if (record.attributeName === 'icon')
                        itemdef.iconValue.set(itemdef.slotdom.icon);
                    else if (record.attributeName === 'title')
                        itemdef.labelValue.set(itemdef.slotdom.title);
                    else if (record.attributeName === 'active' && itemdef.slotdom.active && itemdef.index !== this.#currIndex)
                        this.moveToTabIndex(itemdef.index);
                });
            });
    }

    /**
     * Construct a tab based on an icon and a label
     * @param {string} icon Icon to use
     * @param {string} label Label to put on the tab
     * @param {boolean} [active] Whether this is an active tab or not
     */
    #constructTab = (icon, label, active = false) => {
        return $_.make('button', {
            classList: { 'mdc-tab': true, 'mdc-tab--stacked': true, 'mdc-tab--active': active },
            role: 'tab',
            "aria-selected": active,
            tabindex: 0
        },
            ['span', { class: 'mdc-tab__content' },
                ['span', { class: 'mdc-tab__icon material-icons', 'aria-hidden': true }, icon],
                ['span', { class: 'mdc-tab__text-label' }, label]
            ],
            ['span', { classList: { 'mdc-tab-indicator': true, 'mdc-tab-indicator--active': active } },
                ['span', { class: 'mdc-tab-indicator__content mdc-tab-indicator__content--underline' }]
            ],
            ['span', { class: 'mdc-tab__ripple' }]
        );
    }

    /**
     * Process a tab-bar DOM item into a definition, including a mutation observer and shadowdom construction
     * @param {DrockTabBarItem} item tabbar item to process
     * @param {number} index index of the item
     */
    #createItemTabDef = (item, index) => {
        let mutObs = new MutationObserver(this.#onSlotItemChanged);
        mutObs.observe(item, { attributes: true, attributeFilter: DrockTabBarItem.observedAttributes });

        let iconValue = $_.value(item.icon);
        let labelValue = $_.value(item.title);

        let shadowDom = this.#constructTab(iconValue, labelValue, item.active);

        return {
            index: index,
            slotdom: item,
            mutationObserver: mutObs,
            shadowdom: shadowDom,
            iconValue: iconValue,
            labelValue: labelValue
        };
    }

    /** Setup all of the slots currently present into a full tabbar! */
    #setupAllSlots = () => {
        //Clean up first!
        this.#elementCache.tabBar.forEach(tb => {
            if (tb.mdcTabBar) {
                tb.mdcTabBar.unlisten("MDCTabBar:activated", this.#onTabbarActivate);
                tb.mdcTabBar.destroy();
                tb.mdcTabBar = null;
            }
        });
        this.#elementCache.tabBarContent.empty();

        // Find the items already present in the slot
        let slotElements = this.#elementCache.slot[0].assignedNodes({ flatten: true });
        this.#elementCache.slotchildren =
            slotElements.filter(e => e.tagName === 'DROCK-MDCTABBARITEM');

        //ensure only one active (and make first if none)
        if (this.#elementCache.slotchildren.length < 0) {
            let active = this.#elementCache.slotchildren
                                                .map((item, index) => ({
                                                    item: item,
                                                    index: index
                                                }))
                                                .filter(item => item.item.active);
            if (active.length < 0) {
                this.#elementCache.slotchildren[0].active = true;
                this.#currIndex = 0;
            }
            else if (active.length > 1) {
                this.#currIndex = active[0].index;
                active.slice(1).forEach(item => {
                    item.item.active = false;
                });
            }
            else
                this.#currIndex = active[0].index;
        }

        // create item definitions from the slotted items, including constructing tab buttons
        this.#itemdefs =
            this.#elementCache.slotchildren.map(this.#createItemTabDef);

        // Add the elements
        let constructedTabs = this.#itemdefs
                                    .map(itemdef => itemdef.shadowdom)
                                    .reduce((p, c) => { p.push(...c); return p; }, [])
        this.#elementCache.tabBarContent.appendChildren(...constructedTabs);

        // Now, setup MDC stuff!
        this.#elementCache.tabBar.forEach(tb => {
            let mdcTabBar = new MDCTabBar(tb);
            mdcTabBar.listen("MDCTabBar:activated", this.#onTabbarActivate);
            tb.mdcTabBar = mdcTabBar;
        });
    }

    /**
     * Event triggered when the tabbar changes
     * @param {any} event
     */
    #onTabbarActivate = (event) => {
        this.#currIndex = event.detail.index;

        // update all of the slot children appropriately
        this.#itemdefs.forEach((itemdef, index) => {
            itemdef.slotdom.active = index === event.detail.index;
        });

        // send the big event
        this.dispatchEvent(new CustomEvent('tabbarchange', {
            detail: {
                index: event.detail.index
            },
            bubbles: true,
            composed: true
        }));
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
window.customElements.define('drock-mdctabbar', DrockTabBar);
