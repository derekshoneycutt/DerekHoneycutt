import { Imogene as $, ImogeneExports as $_ } from '../../Imogene/Imogene';
import { MdcIconButton } from './Core/mdciconbutton';
import { DrockCalendarElement } from '../../cmp/calendar';
import { MDCRipple } from '@material/ripple';
import { MDCList } from '@material/list';
import { MDCChipSet } from '@material/chips';
import '../../DateExts';

/** Types of views that are recognized by the base element */
export const ViewTypes = ['agenda', 'day', '3day', 'week', 'month', 'reports'];
/** View icons used in the base element */
const ViewIcons = ['view_agenda', 'view_day', 'view_week', 'view_week', 'view_module', 'view_list'];

/** Component used for the core elements, including topbar and sidebar, of sawtooth */
export default class SawBaseElement extends HTMLElement {
    constructor() {
        super();

        /** Notifying values utilized to bind to the interface */
        this._values = {
            /** Primary title that is shown to the user */
            primaryTitle: $_.value('Sawtooth'),

            /** Values describing which view is currently selected */
            selectedviews: $_.valueArray(6, i => false),
            /** Describes which icon is currently shown as selected */
            currselected: $_.value(0, i => ViewIcons[i]),

            /** Describes the title that should accompany they expander item */
            expandertitle: $_.value(false, v => v ? 'Hide sidebar' : 'Show sidebar')
        };

    }

    connectedCallback() {
        if (this.shadowRoot)
            return;

        //If shadowroot is not already retrieved, create it, copy the template, and setup events & properties

        const shadowRoot = this.attachShadow({ mode: 'open' });
        /** @type {HTMLTemplateElement} */
        const template = $('#saw-base')[0];
        const showChildren = template.content.cloneNode(true);

        const container = $(showChildren, '.saw-base-root');
        const topTitle = $(showChildren, '#saw-toptitle');
        const settings = $(showChildren, '.saw-topbar-settings');

        /** @type {HTMLDivElement[]} */
        const menuitems = $(showChildren, '.saw-sidebar-menu-item .mdc-ripple-surface');
        const mdcchips = $(showChildren, '.mdc-chip');

        const calendarmenu = $(showChildren, '.saw-sidebar-menu-calendar');
        const calicon = $(calendarmenu, '.saw-sidebar-menu-item-icon');
        const todaychipset = $(calendarmenu, '#saw-navchip-today');
        const todaychipchip = $(todaychipset, '#saw-navchip-row-today');
        const todaychip = $(todaychipchip, '#saw-nav-today-chip');
        const navCalendar = $(calendarmenu, '#saw-nav-calendar');

        const fabs = $(showChildren, '.saw-add-button .mdc-fab');
        const fabHide = $(showChildren, '.saw-add-button .saw-add-button-hidedrawer .mdc-fab');
        const addbuttonlist = $(showChildren, '.saw-add-button-drawer-list');
        const addplan = $(addbuttonlist, '#saw-add-plan');
        const addreceipt = $(addbuttonlist, '#saw-add-receipt');
        const addnote = $(addbuttonlist, '#saw-add-note');

        const viewmenu = $(showChildren, '.saw-sidebar-menu-view');
        const viewmenuicon = $(viewmenu, '.saw-sidebar-menu-item-icon');
        const currviewicon = $(viewmenuicon, '#saw-sidebar-menu-view-icon');
        const viewlist = $(viewmenu, '.saw-sidebar-menu-view-list');
        const agenda = $(viewlist, '#saw-view-agenda');
        const day = $(viewlist, '#saw-view-day');
        const threeday = $(viewlist, '#saw-view-3day');
        const week = $(viewlist, '#saw-view-week');
        const month = $(viewlist, '#saw-view-month');
        const reports = $(viewlist, '#saw-view-reports');
        const viewlistitems = [agenda, day, threeday, week, month, reports];

        const sidebarexpander = $(showChildren, '.saw-sidebar-body-expander');
        /** @type {HTMLDivElement[]} */
        const expanderRipples = $(sidebarexpander, '.mdc-ripple-surface');
        const expandedCheck = $(showChildren, '#saw-base-expanded');
        this._expansionCheckbox = expandedCheck;

        $_.appendChildren(topTitle, this._values.primaryTitle);

        $_.addEvents(settings, {
            click: e => console.log('settings')
        });

        menuitems.forEach(mi => new MDCRipple(mi));
        mdcchips.forEach(mc => new MDCRipple(mc));

        const todaychipsetmdc = new MDCChipSet(todaychipset[0]);
        $_.addEvents(calendarmenu, {
            focus: e => {
                setTimeout(() =>
                    $_.setProperties(calicon, {
                        style: {
                            display: 'none'
                        }
                    }),
                    400);
            },
            blur: e => {
                $_.setProperties(calicon, {
                    style: {
                        display: 'block'
                    }
                });
            }
        });
        $_.addEvents(navCalendar, {
            choosedate: e => {
                todaychip[0].blur();
                todaychipchip[0].blur();
                navCalendar[0].blur();
            }
        });

        $_.addEvents(todaychipchip, {
            click: e => $_.setProperties(navCalendar, { selection: Date.today() }),
            keyup: e => {
                if (e.key === ' ')
                    todaychipchip[0].click();
            },
            keydown: e => {
                if (e.key === 'Enter')
                    todaychipchip[0].click();
            }
        });

        fabs.forEach(fab => new MDCRipple(fab));
        $_.addEvents(fabHide, {
            click: () => fabs.forEach(f => f.blur())
        });
        const list = new MDCList(addbuttonlist[0]);
        const listItemRipples = list.listElements.map((listItemEl) => new MDCRipple(listItemEl));

        this.__prepareAddItem(addplan, 'addplan', navCalendar);
        this.__prepareAddItem(addreceipt, 'addreceipt', navCalendar);
        this.__prepareAddItem(addnote, 'addnote', navCalendar);

        $_.setProperties(currviewicon, {
            innerHTML: this._values.currselected
        });
        $_.addEvents(viewmenu, {
            focus: e => {
                setTimeout(() =>
                    $_.setProperties(viewmenuicon, {
                        style: {
                            display: 'none'
                        }
                    }),
                    400);
            },
            blur: e => {
                $_.setProperties(viewmenuicon, {
                    style: {
                        display: 'block'
                    }
                });
            }
        });

        let currviewindex = ViewTypes.indexOf(this.currview);
        if (currviewindex < 0)
            currviewindex = 0;
        this._values.selectedviews[currviewindex].set(true);
        this._values.currselected.set(currviewindex);

        const viewlistmdc = new MDCList(viewlist[0]);
        const viewlistItemRipples = viewlistmdc.listElements.map((listItemEl) => new MDCRipple(listItemEl));

        this.__prepareListItem(agenda, 0, 'agenda');
        this.__prepareListItem(day, 1, 'day');
        this.__prepareListItem(threeday, 2, '3day');
        this.__prepareListItem(week, 3, 'week');
        this.__prepareListItem(month, 4, 'month');
        this.__prepareListItem(reports, 5, 'reports');


        expanderRipples.forEach(er => new MDCRipple(er));
        $_.setProperties(sidebarexpander, {
            title: this._values.expandertitle,
            on: {
                keyup: e => {
                    if (e.key === ' ')
                        sidebarexpander[0].click();
                },
                keydown: e => {
                    if (e.key === 'Enter')
                        sidebarexpander[0].click();
                }
            }
        });
        $_.addEvents(expandedCheck, {
            change: () => {
                this.expanded = expandedCheck[0].checked;
                //this._values.expandertitle.set(expandedCheck[0].checked);
                //this.__setAttribute('expanded', expandedCheck[0].checked);
            }
        });

        shadowRoot.appendChild(showChildren);

        $_.setProperties(todaychip, { tabindex: -1 });
    }

    // When attributes are changed, mostly send it to the values, or handle specifically
    attributeChangedCallback(name, oldValue, newValue) {
        const camelName = $_.camelize(name);
        if (this._values[camelName]) {
            this._values[camelName].set(newValue);
        }

        switch (name) {
            case 'currview':
                this.__updatecurrview(newValue);
                break;
            case 'expanded':
                this.__updateexpanded(newValue);
                break;
        }
    }

    __setAttribute(attr, value) {
        if (value)
            this.setAttribute(attr, value);
        else if (this.hasAttribute(attr))
            this.removeAttribute(attr);
    }

    static get observedAttributes() {
        return ['primary-title', 'currview', 'expanded'];
    }

    /** Primary title to display to the user */
    get primaryTitle() {
        if (this.hasAttribute('primary-title'))
            return this.getAttribute('primary-title');
        return '';
    }
    set primaryTitle(value) {
        this.__setAttribute('primary-title', value);
    }

    /** Current view displayed to the user */
    get currview() {
        if (this.hasAttribute('currview'))
            return this.getAttribute('currview');
        return 'agenda';
    }

    set currview(value) {
        this.__setAttribute('currview', value);
    }

    /** Whether the sidebar is currently expanded or not */
    get expanded() {
        return this.hasAttribute('expanded');
    }
    set expanded(value) {
        this.__setAttribute('expanded', !!value);
    }




    /**
     * Perform the update for when currview attribute is updated
     * @param {string} newValue New value to update currview to
     */
    __updatecurrview(newValue) {
        let currviewindex = ViewTypes.indexOf(newValue.toLowerCase());
        if (currviewindex < 0)
            currviewindex = 0;
        const currsel = this._values.currselected.get();
        if (currsel >= 0)
            this._values.selectedviews[currsel].set(false);
        this._values.selectedviews[currviewindex].set(true);
        this._values.currselected.set(currviewindex);

        let currviewEvent = new CustomEvent('changeview', {
            detail: {
                view: this.currview
            },
            bubbles: true,
            composed: true
        });
        this.dispatchEvent(currviewEvent);
    }

    /**
     * Perform the update for when expanded attribute is updated
     * @param {string} newValue New value to update expanded to
     */
    __updateexpanded(newValue) {
        this._expansionCheckbox[0].checked = !!newValue;
        this._values.expandertitle.set(this._expansionCheckbox[0].checked);

        $_.setStyle(this, {
            '--saw-sidebar-currexpansion': this._expansionCheckbox[0].checked ?
                'var(--saw-sidebar-size-expandoffset, 0rem)' : '0rem'
        });

        let expandEvent = new CustomEvent('expanded', {
            detail: {
                expanded: this._expansionCheckbox[0].checked
            },
            bubbles: true,
            composed: true
        });
        this.dispatchEvent(expandEvent);
    }

    /**
     * Setup proper handling of the Add button items
     * @param {HTMLLIElement[]} listItem List item to modify
     * @param {string} eventname Event name to raise when the item is clicked
     * @param {DrockCalendarElement[]} navCalendar navigation calendar components
     */
    __prepareAddItem(listItem, eventname, navCalendar) {
        $_.setProperties(listItem, {
            on: {
                click: () => {
                    listItem[0].blur();
                    let event = new CustomEvent(eventname, {
                        detail: {
                            currdate: navCalendar[0].selection
                        },
                        bubbles: true,
                        composed: true
                    });
                    this.dispatchEvent(event);
                },
                keyup: e => {
                    if (e.key === ' ')
                        listItem[0].click();
                },
                keydown: e => {
                    if (e.key === 'Enter')
                        listItem[0].click();
                }
            }
        });
    }

    /**
     * Prepare list items for appropriate display
     * @param {HTMLLIElement[]} listItem list item to modify
     * @param {number} itemIndex index of the item to modify
     * @param {string} view the view that the item represents
     */
    __prepareListItem(listItem, itemIndex, view) {
        $_.setProperties(listItem, {
            classList: {
                'mdc-list-item--selected': this._values.selectedviews[itemIndex]
            },
            on: {
                click: () => {
                    this.currview = view;
                    listItem[0].blur();
                },
                keyup: e => {
                    if (e.key === ' ')
                        listItem[0].click();
                },
                keydown: e => {
                    if (e.key === 'Enter')
                        listItem[0].click();
                }
            }
        });
    }
}
window.customElements.define('saw-base', SawBaseElement);
