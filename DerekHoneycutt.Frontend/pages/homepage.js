import { Imogene as $_ } from '../Imogene/Imogene';
import { MDCRipple } from '@material/ripple';

// Homepage item can be a super simple, easy thing! :D
export class DrockHomepageItem extends HTMLElement {
    constructor() {
        super();
    }

    static get observedAttributes() {
        return ['icon', 'title', 'subtitle', 'href'];
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

    /** Subtitle to display to the user */
    get subtitle() {
        return this.getAttribute('subtitle') || '';
    }
    set subtitle(value) {
        this.#setAttribute('subtitle', value);
    }

    /** Location that the page that the item links to */
    get href() {
        return this.getAttribute('href') || '';
    }
    set href(value) {
        this.#setAttribute('href', value);
    }

    /** Send an event to navigate */
    navigate(evt) {
        // send the big event
        const eventAllowed = this.dispatchEvent(new CustomEvent('navigate', {
            bubbles: true,
            composed: true,
            cancelable: true
        }));
        return eventAllowed;
    }
}
window.customElements.define('drock-homepageitem', DrockHomepageItem);

/** Component for showing a home page */
export default class DrockHomepage extends HTMLElement {
    #elementCache = {
        homecontainer: $_.makeEmpty(),
        listcontainer: $_.makeEmpty(),
        slot: $_.makeEmpty(),
        slotchildren: $_.makeEmpty()
    };
    #itemdefs = [];

    constructor() {
        super();
    }

    connectedCallback() {
        // Only run this once from here
        if (this.shadowRoot)
            return;

        // Get the shadow root and template data
        const shadowRoot = $_.enhance(this.attachShadow({ mode: 'open' }));
        shadowRoot.appendChildren($_.find('#drock-homepage').prop('content').cloneNode(true));

        this.#elementCache = {
            homecontainer: shadowRoot.find('.home-landing-div'),
            listcontainer: shadowRoot.find('.home-page-list'),
            slot: shadowRoot.find('slot'),

            slotchildren: []
        };

        // Setup the homepage list slots!
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
                        itemdef.titleValue.set(itemdef.slotdom.title);
                    else if (record.attributeName === 'subtitle')
                        itemdef.subtitleValue.set(itemdef.slotdom.subtitle);
                    else if (record.attributeName === 'href')
                        itemdef.hrefValue.set(itemdef.slotdom.href);
                });
            });
    }

    /**
     * Construct a item based on an icon and a label
     * @param {DrockHomepageItem} item original item to build off of
     * @param {string} icon Icon to use
     * @param {string} title Title to put on the item
     * @param {string} subtitle subtitle to show for the item
     * @param {string} href address to link to
     */
    #constructItem = (item, icon, title, subtitle, href) => {
        let link;
        const ret = $_.make('div', { class: 'home-page-list-item' },
            link = $_.make('a', {
                class: 'home-page-list-link mdc-ripple-surface',
                href: href,
                on: {
                    click: e => {
                        if (!item.navigate(e)) {
                            e.preventDefault();
                        }
                    }
                }
            },
                ['div', { class: 'home-page-icons' },
                    ['span', { class: 'home-page-icon-actual material-icons' }, icon]
                ],
                ['div', { class: 'home-page-listing' },
                    ['div', { class: 'home-page-listing-title' }, title],
                    ['div', { class: 'home-page-listing-subtitle' }, subtitle]
                ]
            )
        );
        link.mdcRipple = new MDCRipple(link[0]);
        return ret;
    }

    /**
     * Process a home page DOM item into a definition, including a mutation observer and shadowdom construction
     * @param {DrockHomepageItem} item homepage item to process
     * @param {number} index index of the item
     */
    #createItemDef = (item, index) => {
        let mutObs = new MutationObserver(this.#onSlotItemChanged);
        mutObs.observe(item, { attributes: true, attributeFilter: DrockHomepageItem.observedAttributes });

        let iconValue = $_.value(item.icon);
        let titleValue = $_.value(item.title);
        let subtitleValue = $_.value(item.subtitle);
        let hrefValue = $_.value(item.href);

        let shadowDom = this.#constructItem(item, iconValue, titleValue, subtitleValue, hrefValue);

        return {
            index: index,
            slotdom: item,
            mutationObserver: mutObs,
            shadowdom: shadowDom,
            iconValue: iconValue,
            titleValue: titleValue,
            subtitleValue: subtitleValue,
            hrefValue: hrefValue
        };
    }

    /** Setup all of the slots currently present into a full homepage! */
    #setupAllSlots = () => {
        //Clean up first!
        this.#elementCache.listcontainer.empty();

        // Find the items already present in the slot
        let slotElements = this.#elementCache.slot[0].assignedNodes({ flatten: true });
        this.#elementCache.slotchildren =
            slotElements.filter(e => e.tagName === 'DROCK-HOMEPAGEITEM');

        // create item definitions from the slotted items, including constructing items
        this.#itemdefs =
            this.#elementCache.slotchildren.map(this.#createItemDef);

        // Add the elements
        let constructedItems = this.#itemdefs
            .map(itemdef => itemdef.shadowdom)
            .reduce((p, c) => { p.push(...c); return p; }, [])
        this.#elementCache.listcontainer.appendChildren(...constructedItems);
    }
}
window.customElements.define('drock-homepage', DrockHomepage);
