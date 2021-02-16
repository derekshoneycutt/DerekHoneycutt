import { Imogene as $, ImogeneExports as $_, ImogeneTemplate as $t } from '../../../Imogene/Imogene';
import { SawDayHeadElement } from './dayhead';
import '../../../DateExts';

/** Representation of an allocation container and slot within a week view */
class SawWeekAllocContainer {
    constructor(cspan = 1, sdate = null, sindex = 0) {

        this._colspan = cspan;
        this._startdate = sdate || Date.today();
        this._slotindex = sindex;
        this._slot = null;
        this._tablecell = null;
    }

    /** Number of columns the event should take up in the week */
    get colspan() {
        return this._colspan;
    }
    set colspan(value) {
        this._colspan = parseInt(value) || 1;
    }

    /** The date that the event starts on */
    get startdate() {
        return this._startdate;
    }
    set startdate(value) {
        this._startdate = new Date(value || Date.today()).getMidnight();
    }

    /** The index of the slot that should be used for the allocation */
    get slotindex() {
        return this._slotindex;
    }
    set slotindex(value) {
        this._slotindex = parseInt(value) || 0;
    }

    /** The rendered slot, if available */
    get slot() {
        return this._slot;
    }
    set slot(value) {
        this._slot = value;
    }

    /** The rendered table cell element, if available */
    get tablecell() {
        return this._tablecell;
    }
    set tablecell(value) {
        this._tablecell = value;
    }
}

/** Component used to do various versions of a week view */
export default class SawWeekElement extends HTMLElement {
    constructor() {
        super();

        this._values = {
            days: $_.value(this.getAttribute('days') || 3),
            basedon: $_.value(this.getAttribute('basedon') || Date.today(), v => new Date(v)),

            headrowItems: $_.value(),
            bodyItems: $_.value()
        };

        const shadowRoot = this.attachShadow({ mode: 'open' });

        /** @type {HTMLTemplateElement} */
        const template = $('#saw-week')[0];
        const showChildren = template.content.cloneNode(true);

        this._headrow = $(showChildren, '#saw-week-head-row');
        $_.emptyAndReplace(this._headrow, this._values.headrowItems);
        this._body = $(showChildren, '#saw-week-body');
        $_.emptyAndReplace(this._body, this._values.bodyItems);

        this.__currDays = 0;
        this.__currBase = new Date(0);
        this.__totalrows = 0;
        this.__totalsSlots = [];
        this.__allocrows = [];
        this.__allocslots = [];
        this.__transrows = [];
        this.__transslots = [];

        shadowRoot.appendChild(showChildren);
    }

    /** Number of days to show in the week */
    get days() {
        return this._values.days.get();
    }
    set days(value) {
        this.__setAttribute('days', parseInt(value) || 3);
    }

    /** Date that the current view is based on */
    get basedon() {
        return this._values.basedon.get();
    }
    set basedon(value) {
        this.__setAttribute('basedon', value);
    }

    static get observedAttributes() {
        return [ 'days', 'basedon' ];
    }

    attributeChangedCallback(name, oldValue, newValue) {
        const camelName = $_.camelize(name);
        if (this._values[camelName]) {
            this._values[camelName].set(newValue);
        }

        /*switch (name) {
            case 'days':
                this.__constructDays();
                break;
        }*/
    }

    __setAttribute(attr, value) {
        if (value)
            this.setAttribute(attr, value);
        else if (this.hasAttribute(attr))
            this.removeAttribute(attr);
    }

    /**
     * Construct the totals slots in the table
     * @param {Array<HTMLTableRowElement>} bodyItems array to add the constructed rows to
     * @param {number} days Number of days to show in the table
     */
    __constructTotalSlots(bodyItems, days) {
        let totalsSlots = [];
        for (let t = 0; t < this.__totalrows; ++t) {
            let rowItem = $t`<tr></tr>`;
            for (let j = 0; j < days; ++j) {
                let slot = $t`<slot name="total${totalsSlots.length + 1}"></slot>`;
                let row = $t`<td>${slot}</td>`;
                totalsSlots.push(slot);
                rowItem.append(row);
            }
            bodyItems.push(rowItem);
        }
        this.__totalsSlots = totalsSlots;
    }

    /**
     * Construct allocation slots in the table
     * @param {Array<HTMLTableRowElement>} bodyItems array to add the constructed rows to
     */
    __constructAllocSlots(bodyItems) {
        let allocSlots = [];
        for (let ar = 0; ar < this.__allocrows.length; ++ar) {
            let row = this.__allocrows[ar].reduce((p, c) => {
                if (!p.includes(c))
                    p.push(c);
                return p;
            }, []);
            let rowItem = $t`<tr class="${ar === 0 ? "saw-allocs-row-first" : ""}"></tr>`;
            for (let ac = 0; ac < row.length; ++ac) {
                if (row[ac].slotindex > 0)
                    row[ac].slot = $t`<slot name="alloc${row[ac].slotindex}"></slot>`;
                let cell = $t`<td colspan="${row[ac].colspan}">${row[ac].slot}</td>`;
                row[ac].tablecell = cell;
                rowItem.appendChild(cell);
                allocSlots.push(row[ac].slot);
            }
            bodyItems.push(rowItem);
        }
        this.__allocslots = allocSlots;
    }

    /**
     * Construct transaction slots in the table
     * @param {Array<HTMLTableRowElement>} bodyItems array to add the constructed rows to
     */
    __constructTransSlots(bodyItems) {
        let transSlots = [];
        for (let tr = 0; tr < this.__transrows.length; ++tr) {
            let row = this.__transrows[tr];
            let rowItem = $t`<tr class="${ar === 0 ? "saw-trans-row-first" : ""}"></tr>`;
            for (let tc = 0; tc < row.length; ++tc) {
                let cell = $t`<td></td>`;
                if (row[tc] > 0) {
                    let slot = $t`<slot name="trans${transSlots.length}"></slot>`;
                    cell.appendChild(slot);
                    transSlots.push({ tablecell: cell, slot: slot });
                }
                rowItem.appendChild(cell);
            }
            bodyItems.push(rowItem);
        }
        this.__transslots = transSlots;
    }

    /**
     * Construct the various days within the table
     * @param {Date} basedon Date that the week is based on
     * @param {Date} start Date that the week begins on
     * @param {Date} end Date that the week ends on
     * @param {number} days Number of days shown
     */
    __constructDays(basedon, start, end, days) {
        this.style.setProperty('--saw-week-count-days', days);

        let headItems = [];
        for (let i = 0; i < days; ++i) {
            let currDate = new Date(start);
            currDate.setDate(currDate.getDate() + i);
            let headItem =
$t`<th><saw-day-head dayofweek="${currDate.getDay() || '0'}" day="${currDate.getDate()}"></saw-day-head></th>`;
            headItems.push(headItem);
        }
        this._values.headrowItems.set(headItems);

        let bodyItems = [];

        this.__constructTotalSlots(bodyItems, days);
        this.__constructAllocSlots(bodyItems);
        this.__constructTransSlots(bodyItems);

        bodyItems.push($t`<tr class="saw-week-bottom"></tr>`);
        this._values.bodyItems.set(bodyItems);

        this.__currDays = days;
        this.__currBase = basedon;
    }

    /**
     * Find the row an allocation fits in or create a new row for it
     * @param {Date} start Date that the week starts on
     * @param {number} dateon Index of days in the week that the event starts on
     * @param {number} colspan Number of columns the event is to span in this week
     * @returns {number} Row index that allocation should occupy
     */
    __findRowFitOrMake(start, dateon, colspan) {
        let userow = -1;
        for (let i = 0; i < this.__allocrows.length; ++i) {
            const startcell = this.__allocrows[i][dateon];
            if (startcell.slotindex === 0) {
                let hasroom = true;
                if (colspan > 1) {
                    for (let j = dateon + 1; j < dateon + colspan; ++j) {
                        if (this.__allocrows[i][j].slotindex !== 0) {
                            hasroom = false; break;
                        }
                    }
                }
                if (hasroom) {
                    userow = i;
                    break;
                }
            }
        }

        if (userow < 0) {
            userow = this.__allocrows.length;
            let newrow = [];
            for (let i = 0; i < this.days; ++i)
                newrow.push(new SawWeekAllocContainer(1, start.plusDays(i), 0));
            this.__allocrows.push(newrow);
        }

        return userow;
    }

    /**
     * Find the most appropriate row for a given transaction, making a new row if needed
     * @param {number} dateon What day of this week the proposed transaction should live in
     * @returns {number} index of the row that the transaction should live in
     */
    __findTransFitOrMake(dateon) {
        let userow = -1;
        for (let i = 0; i < this.__transrows.length; ++i) {
            const cell = this.__transrows[i][dateon];
            if (cell === 0) {
                userow = i;
                break;
            }
        }

        if (userow < 0) {
            userow = this.__transrows.length;
            let newrow = [];
            for (let i = 0; i < this.days; ++i)
                newrow.push(0);
            this.__transrows.push(newrow);
        }

        return userow;
    }

    /**
     * Prepare an allocation for whatever row is most appropriate to it
     * @param {Date} start Date that this week starts on
     * @param {Date} startdate Date that the allocation starts on
     * @param {Date} enddate Date that the allocation ends on
     * @param {number} slotnum Proposed slot number for the allocation
     */
    __prepareAllocRow(start, startdate, enddate, slotnum) {
        let dateon = start.compareDate(startdate);
        if (dateon < this.days) {
            const colspan = startdate.compareDate(enddate) + 1;
            let truecolspan = colspan;
            if (dateon < 0) {
                if (truecolspan > dateon * -1) {
                    truecolspan += dateon;
                    dateon = 0;
                }
                else return;
            }
            if (dateon + truecolspan >= this.days)
                truecolspan = this.days - dateon;

            const userow = this.__findRowFitOrMake(start, dateon, truecolspan);

            let modRow = this.__allocrows[userow];
            let newSlot =
                new SawWeekAllocContainer(
                    truecolspan,
                    startdate.getMidnight(),
                    slotnum);
            for (let i = dateon; i < dateon + truecolspan; ++i) {
                modRow[i] = newSlot;
            }
        }
    }

    /**
     * Prepare a slot for a transaction, making a new row as needed
     * @param {Date} start Date that this week begins on
     * @param {Date} date Date that the transaction should live in
     * @param {number} slotnum Proposed slot number for the transaction
     */
    __prepareTransRow(start, date, slotnum) {
        const dateon = new Date(start).compareDate(date);
        if (dateon < this.days) {
            const userow = this.__findTransFitOrMake(dateon);

            let modRow = this.__transrows[userow];
            modRow[dateon] = slotnum;
        }
    }

    /**
     * Prepare the table rows for the week
     * @param {number} totalCount Number of totals to include per day
     * @param {Array} allocs Allocations that will be displayed in the week
     * @param {Array} transacts Transactions that will be displayed in the week
     * @returns {[number, number, number]} Number of slots for totals, allocations, and transactions created
     */
    prepareRows(totalCount, allocs = [], transacts = []) {
        const basedon = new Date(this.basedon || Date.today());
        const days = parseInt(this.days) || 3;
        const start = days !== 7 ? basedon :
            (() => {
                let r = new Date(basedon); r.setDate(r.getDate() - r.getDay()); return r;
            })();
        const end = (() => {
            let r = new Date(start); r.setDate(start.getDate() + days); return r;
        })();

        this.__totalrows = totalCount;

        this.__allocrows = [];
        this.__allocslots = [];
        allocs.forEach((v, i) => this.__prepareAllocRow(start, v.primaryDate, v.endDate, i + 1));

        this.__transrows = [];
        this.__transslots = [];
        transacts.forEach((v, i) => this.__prepareTransRow(start, v.primaryDate, i + 1));

        this.__constructDays(basedon, start, end, days);
        return [this.__totalrows * this.__currDays, allocs.length, transacts.length];
    }
}
window.customElements.define('saw-week', SawWeekElement);
