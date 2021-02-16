import { Imogene as $, ImogeneExports as $_ } from '../Imogene/Imogene';
import { MDCRipple } from '@material/ripple';
import * as defs from '../localdefs';
import '../DateExts';

/** Formatting util used to get month names */
const monthNameFormatter = Intl.DateTimeFormat(defs.currlocal, { month: 'long' });

/** Component used to display a navigable calendar */
export default class DrockCalendarElement extends HTMLElement {
    constructor() {
        super();

        this._values = {
            usemonth: $_.value(Date.today()),
            selection: $_.value(Date.today()),

            monthhead: $_.value(''),

            days: $_.valueArray(42),
            dates: $_.valueArray(42),
            istoday: $_.valueArray(42, v => false),
            isoutmonth: $_.valueArray(42, v => false),
            isselected: $_.valueArray(42, v => false)
        };

        this._currSelected = null;

        this.setMonthView();

    }

    connectedCallback() {
        if (this.shadowRoot)
            return;

        const shadowRoot = this.attachShadow({ mode: 'open' });
        /** @type {HTMLTemplateElement} */
        const template = $('#drock-calendar')[0];
        const showChildren = template.content.cloneNode(true);
        shadowRoot.appendChild(showChildren);

        const monthheadEl = $(shadowRoot, '.drock-cal-month');
        $_.appendChildren(monthheadEl, this._values.monthhead);

        const caldaytexts = $(shadowRoot, '.drock-cal-day__text');
        this._values.dates.forEach((date, i) => {
            const dateEl = caldaytexts[i];//$(shadowRoot, `#drock-cal-day${i + 1}`);
            $_.appendChildren(dateEl, this._values.days[i]);
            const parentEls = $_.parentElements(dateEl);
            $_.setProperties(parentEls, {
                'data-date': date,
                classList: {
                    'drock-cal-day-today': this._values.istoday[i],
                    'drock-cal-day-outmonth': this._values.isoutmonth[i],
                    'drock-cal-day-selected': this._values.isselected[i]
                },
                on: {
                    click: (e) => this.selection = date.get(),
                    keyup: e => {
                        if (e.key === ' ')
                            parentEls[0].click();
                    },
                    keydown: e => {
                        if (e.key === 'Enter')
                            parentEls[0].click();
                    }
                }
            });
        });

        $(shadowRoot, '.mdc-ripple-surface').forEach(ripple => {
            const mdcripple = new MDCRipple(ripple.parentElement);
        });

        const prevButton = $(shadowRoot, '.drock-cal-nav-prev');
        $_.addEvents(prevButton, {
            click: e => {
                let goMonth = new Date(this.usemonth || Date.now());
                goMonth.setMonth(goMonth.getMonth() - 1);
                this.usemonth = goMonth;
            },
            keyup: e => {
                if (e.key === ' ')
                    prevButton[0].click();
            },
            keydown: e => {
                if (e.key === 'Enter')
                    prevButton[0].click();
            }
        });

        const nextButton = $(shadowRoot, '.drock-cal-nav-next');
        $_.addEvents(nextButton, {
            click: e => {
                let goMonth = new Date(this.usemonth || Date.now());
                goMonth.setMonth(goMonth.getMonth() + 1);
                this.usemonth = goMonth;
            },
            keyup: e => {
                if (e.key === ' ')
                    nextButton[0].click();
            },
            keydown: e => {
                if (e.key === 'Enter')
                    nextButton[0].click();
            }
        });
    }

    attributeChangedCallback(name, oldValue, newValue) {
        const camelName = $_.camelize(name);
        if (this._values[camelName]) {
            this._values[camelName].set(newValue);
        }

        switch (name) {
            case 'usemonth':
                this.onChangeMonth(oldValue, newValue);
                break;
            case 'selection':
                this.onSelection(oldValue, newValue);
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
        return [ 'usemonth', 'selection' ];
    }

    /** Current month beig displayed */
    get usemonth() {
        if (this.hasAttribute('usemonth'))
            return this.getAttribute('usemonth');
        return Date.today();
    }
    set usemonth(value) {
        this.__setAttribute('usemonth', value || Date.today());
    }

    /** Current date selected on the calendar */
    get selection() {
        if (this.hasAttribute('selection'))
            return this.getAttribute('selection');
        return Date.today();
    }
    set selection(value) {
        this.__setAttribute('selection', value || Date.today());
    }


    /**
     * Event to run when the month has changed
     * @param {string} oldValue The previous value of usemonth
     * @param {string} newValue The new value of usemonth
     */
    onChangeMonth(oldValue, newValue) {
        if (oldValue !== newValue) {
            this.setMonthView();

            let event = new CustomEvent('changemonth', {
                detail: {
                    month: this.usemonth
                },
                bubbles: true,
                composed: true
            });
            this.dispatchEvent(event);
        }
    }

    /**
     * Event run when the selection has been changed to a new date
     * @param {string} oldValue old selection, if present
     * @param {string} newValue new selection
     */
    onSelection(oldValue, newValue) {
        if (oldValue !== newValue) {
            var prevusemonth = new Date(this.usemonth || Date.now());
            var useselection = new Date(this.selection || Date.now());
            if (prevusemonth.getFullYear() !== useselection.getFullYear() ||
                prevusemonth.getMonth() !== useselection.getMonth()) {
                this.usemonth = useselection;
            }
            else {
                if (this._currSelected)
                    this._values.isselected[this._currSelected].set(false);

                let currDate = new Date(useselection);
                currDate.setDate(1);
                currDate.setDate((-1 * + currDate.getDay()) + 1);
                if (currDate.getDate() === 1)
                    currDate.setDate(-6);
                for (let day = 0; day < 42; ++day) {
                    if (useselection.getFullYear() === currDate.getFullYear() &&
                        useselection.getMonth() === currDate.getMonth() &&
                        useselection.getDate() === currDate.getDate()) {
                        this._values.isselected[day].set(true);
                        this._currSelected = day;
                    }

                    currDate.setDate(currDate.getDate() + 1);
                }
            }

            let event = new CustomEvent('choosedate', {
                detail: {
                    date: useselection
                },
                bubbles: true,
                composed: true
            });
            this.dispatchEvent(event);
        }
    }

    /**
     * Get the string to show as the header for the current month
     * @param {Date} fromDate date to get the header from
     * @returns {string} string heading the current month shown
     */
    getMonthHeadContent(fromDate) {
        let useMonthDate = new Date(fromDate);
        let monthStr = '';
        try {
            monthStr = monthNameFormatter.format(useMonthDate);
        }
        catch (err) {
            monthStr = monthNameFormatter.format(Date.now());
        }
        return `${monthStr} ${useMonthDate.getFullYear()}`;
    }

    /** Update what is currently shown to the current state */
    setMonthView() {
        let useMonthDate = new Date(this.usemonth || Date.now());
        let selectionDate = new Date(this.selection || Date.now());

        this._values.monthhead.set(this.getMonthHeadContent(useMonthDate));

        let currDate = new Date(useMonthDate);
        currDate.setDate(1);
        currDate.setDate((-1 * + currDate.getDay()) + 1);
        if (currDate.getDate() === 1)
            currDate.setDate(-6);
        for (let day = 0; day < 42; ++day) {
            let newDate = new Date(currDate);

            this._values.dates[day].set(newDate);
            this._values.days[day].set(newDate.getDate());
            this._values.istoday[day].set(newDate.isToday());
            this._values.isoutmonth[day].set(newDate.getMonth() !== useMonthDate.getMonth());
            const isselected = !newDate.isSameDateAs(selectionDate);
            this._values.isselected[day].set(isselected);
            if (isselected)
                this._currSelected = day;

            currDate.setDate(currDate.getDate() + 1);
        }
    }
}
window.customElements.define('drock-calendar', DrockCalendarElement);
