import { Imogene as $, ImogeneExports as $_ } from '../../Imogene/Imogene';
import { MDCTabBar } from '@material/tab-bar';
import { MDCRipple } from '@material/ripple';

/** Component for showing the topbar of my portfolio */
export class DrockTopBar extends HTMLElement {
    constructor() {
        super();

        const self = this;

        const shadowRoot = this.attachShadow({ mode: 'open' });
        /** @type {HTMLTemplateElement} */
        const template = $('#drock-topbar')[0];
        const showChildren = template.content.cloneNode(true);

        /** @type {HTMLDivElement[]} */
        const tabBar = $(showChildren, '.mdc-tab-bar');
        tabBar.forEach(tb => {
            let mdcTabBar = new MDCTabBar(tb);
            mdcTabBar.listen("MDCTabBar:activated", e => {
                self.onTabbarActivate(e);
            });
            //mdcTabBar.activateTab(2);
            tabBar.mdcTabBar = mdcTabBar;
        });
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

    onTabbarActivate(event) {
        //if (event.detail.index === 5)
        //    window.location.href = "https://subaruvagabond.com/";
        console.log(`Activated: ${event.detail.index}`);
    }
}
window.customElements.define('drock-topbar', DrockTopBar);