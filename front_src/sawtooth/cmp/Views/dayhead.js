import { Imogene as $, ImogeneExports as $_, ImogeneTemplate as $t } from '../../../Imogene/Imogene';
import '../../../DateExts';

/** Names of days to show in the header area */
const dayNames = ['SUN', 'MON', 'TUE', 'WED', 'THU', 'FRI', 'SAT'];

/** Component showing header information for a given day */
export default class SawDayHeadElement extends HTMLElement {
    constructor() {
        super();

        this._values = {
            day: $_.value(this.getAttribute('day') || 1),
            dayofweek: $_.value(this.getAttribute('dayofweek') || 0, v => dayNames[v])
        };

        const shadowRoot = this.attachShadow({ mode: 'open' });

        /** @type {HTMLTemplateElement} */
        const template = $('#saw-day-head')[0];
        const showChildren = template.content.cloneNode(true);

        this._namediv = $(showChildren, '.saw-day-head-date-name');
        this._numdiv = $(showChildren, '.saw-day-head-date-num');

        $_.emptyAndReplace(this._namediv, this._values.dayofweek);
        $_.emptyAndReplace(this._numdiv, this._values.day);

        shadowRoot.appendChild(showChildren);
    }

    /** The number day to show */
    get day() {
        return this._values.day.get();
    }
    set day(value) {
        this.__setAttribute('day', parseInt(value) || 1);
    }

    /** The day of the week to show, as a number */
    get dayofweek() {
        return this._values.dayofweek.get();
    }
    set dayofweek(value) {
        this.__setAttribute('dayofweek', parseInt(value) || 0);
    }

    static get observedAttributes() {
        return ['day', 'dayofweek'];
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

}
window.customElements.define('saw-day-head', SawDayHeadElement);

