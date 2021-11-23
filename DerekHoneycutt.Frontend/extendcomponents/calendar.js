import { Imogene as $_ } from '../Imogene/Imogene';
import { MDCRipple } from '@material/ripple';
import * as defs from '../localdefs';
import '../DateExts';

/** Formatting util used to get month names */
const monthNameFormatter = Intl.DateTimeFormat(defs.currlocal, { month: 'long' });

/** Component used to display a navigable calendar */
export default class DrockCalendarElement extends HTMLElement {
    #values = {
        usemonth: $_.value(Date.today()),
        selection: $_.value(Date.today()),

        monthhead: $_.value(''),

        days: $_.valueArray(42),
        dates: $_.valueArray(42),
        istoday: $_.valueArray(42, v => false),
        isoutmonth: $_.valueArray(42, v => false),
        isselected: $_.valueArray(42, v => false)
    };
    #elementCache = {
        monthheadEl: $_.makeEmpty(),
        caldaytexts: $_.makeEmpty(),
        ripples: $_.makeEmpty(),
        prevButton: $_.makeEmpty(),
        nextButton: $_.makeEmpty()
    };
    #mdcCache = {
        ripples: []
    };
    #currSelected = null;
    
    constructor() {
        super();

        this.setMonthView();

    }

    connectedCallback() {
        // Only run this once from here
        if (this.shadowRoot)
            return;

        // Get the shadow root and template data
        const shadowRoot = $_.enhance(this.attachShadow({ mode: 'open' }));
        shadowRoot.appendChildren($_.find('#drock-calendar').prop('content').cloneNode(true));

        this.#elementCache = {
            monthheadEl: shadowRoot.find('.drock-cal-month'),
            caldaytexts: shadowRoot.find('.drock-cal-day__text'),
            ripples: shadowRoot.find('.mdc-ripple-surface'),
            prevButton: shadowRoot.find('.drock-cal-nav-prev'),
            nextButton: shadowRoot.find('.drock-cal-nav-next')
        };

        $_.appendChildren(this.#elementCache.monthheadEl, this.#values.monthhead);

        this.#values.dates.forEach((date, i) => {
            const dateEl = this.#elementCache.caldaytexts[i];
            $_.appendChildren(dateEl, this.#values.days[i]);
            const parentEls = $_.parentElements(dateEl);
            $_.setProperties(parentEls, {
                'data-date': date,
                classList: {
                    'drock-cal-day-today': this.#values.istoday[i],
                    'drock-cal-day-outmonth': this.#values.isoutmonth[i],
                    'drock-cal-day-selected': this.#values.isselected[i]
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

        this.#mdcCache.ripples = this.#elementCache.ripples.map(ripple => new MDCRipple(ripple.parentElement));

        this.#elementCache.prevButton.addEvents({
            click: e => {
                let goMonth = new Date(this.usemonth || Date.now());
                goMonth.setMonth(goMonth.getMonth() - 1);
                this.usemonth = goMonth;
            },
            keyup: e => {
                if (e.key === ' ')
                    this.#elementCache.prevButton[0].click();
            },
            keydown: e => {
                if (e.key === 'Enter')
                    this.#elementCache.prevButton[0].click();
            }
        });

        this.#elementCache.nextButton.addEvents({
            click: e => {
                let goMonth = new Date(this.usemonth || Date.now());
                goMonth.setMonth(goMonth.getMonth() + 1);
                this.usemonth = goMonth;
            },
            keyup: e => {
                if (e.key === ' ')
                    this.#elementCache.nextButton[0].click();
            },
            keydown: e => {
                if (e.key === 'Enter')
                    this.#elementCache.nextButton[0].click();
            }
        });
    }

    attributeChangedCallback(name, oldValue, newValue) {
        const camelName = $_.camelize(name);
        if (this.#values[camelName]) {
            this.#values[camelName].set(newValue);
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

    #setAttribute = (attr, value) => {
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
        this.#setAttribute('usemonth', value || Date.today());
    }

    /** Current date selected on the calendar */
    get selection() {
        if (this.hasAttribute('selection'))
            return this.getAttribute('selection');
        return Date.today();
    }
    set selection(value) {
        this.#setAttribute('selection', value || Date.today());
    }


    /**
     * Event to run when the month has changed
     * @param {string} oldValue The previous value of usemonth
     * @param {string} newValue The new value of usemonth
     */
    onChangeMonth = (oldValue, newValue) => {
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
    onSelection = (oldValue, newValue) => {
        if (oldValue !== newValue) {
            var prevusemonth = new Date(this.usemonth || Date.now());
            var useselection = new Date(this.selection || Date.now());
            if (prevusemonth.getFullYear() !== useselection.getFullYear() ||
                prevusemonth.getMonth() !== useselection.getMonth()) {
                this.usemonth = useselection;
            }
            else {
                if (this.#currSelected)
                    this.#values.isselected[this.#currSelected].set(false);

                let currDate = new Date(useselection);
                currDate.setDate(1);
                currDate.setDate((-1 * + currDate.getDay()) + 1);
                if (currDate.getDate() === 1)
                    currDate.setDate(-6);
                for (let day = 0; day < 42; ++day) {
                    if (useselection.getFullYear() === currDate.getFullYear() &&
                        useselection.getMonth() === currDate.getMonth() &&
                        useselection.getDate() === currDate.getDate()) {
                        this.#values.isselected[day].set(true);
                        this.#currSelected = day;
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
    getMonthHeadContent = (fromDate) => {
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
    setMonthView = () => {
        let useMonthDate = new Date(this.usemonth || Date.now());
        let selectionDate = new Date(this.selection || Date.now());

        this.#values.monthhead.set(this.getMonthHeadContent(useMonthDate));

        let currDate = new Date(useMonthDate);
        currDate.setDate(1);
        currDate.setDate((-1 * + currDate.getDay()) + 1);
        if (currDate.getDate() === 1)
            currDate.setDate(-6);
        for (let day = 0; day < 42; ++day) {
            let newDate = new Date(currDate);

            this.#values.dates[day].set(newDate);
            this.#values.days[day].set(newDate.getDate());
            this.#values.istoday[day].set(newDate.isToday());
            this.#values.isoutmonth[day].set(newDate.getMonth() !== useMonthDate.getMonth());
            const isselected = !newDate.isSameDateAs(selectionDate);
            this.#values.isselected[day].set(isselected);
            if (isselected)
                this.#currSelected = day;

            currDate.setDate(currDate.getDate() + 1);
        }
    }
}
window.customElements.define('drock-calendar', DrockCalendarElement);
