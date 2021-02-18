import { Imogene as $, ImogeneExports as $_, ImogeneTemplate as $t } from './Imogene/Imogene';

/** Main controller class for managing the porfolio interface */
export default class DrockMainController {
    constructor() {
        this.homefetch;
        this.tabs = [];
        this.pages = [];

        this.swipeBase = null;
        this.mainNav = null;

        this.contactDialog = null;
        this.contactEl = null;
        this.contactFab = null;

        this.UrlHandler = { landing: 0, page: 0 };
    }

    /** Watch URL for changes and handle accordingly */
    watchUrl() {
        //Setup a URL Handler to deal with URL changes for navigation
        let params = new URLSearchParams(document.location.search.substring(1));
        this.UrlHandler.landing = parseInt(params.get("landing"), 0) || 0;
        this.UrlHandler.page = parseInt(params.get("page"), 0) || 0;

        //When the history changes (e.g. going back/forward), see if we can just navigate there!
        window.onpopstate = event => {
            if (event.state) {
                this.UrlHandler.landing = event.state.landing;
                this.UrlHandler.page = event.state.page;
            }
            else {
                this.UrlHandler.landing = 0;
                this.UrlHandler.page = 0;
            }

            this.moveLanding(this.UrlHandler.landing, this.UrlHandler.page, true);
        };

        return this;
    }

    /** Fill the elements in the controller based on those in the document */
    findElements() {
        this.swipeBase = $("#swipe-base")[0];
        this.mainNav = $("#drock-main-nav")[0];
        this.contactDialog = $("#drock-contact-dialog")[0];
        this.contactEl = $("#drock-contact-dlg")[0];
        this.contactFab = $('#drock-contactfab')[0];

        $_.addEvents(this.contactDialog, {
            requestclose: e => {
                $_.setClassList(this.contactDialog, {
                    active: false
                });
                this.contactEl.clearFields();
            }
        });

        $_.addEvents(this.contactFab, {
            click: () => {
                $_.setClassList(this.contactDialog, {
                    active: true
                });
            }
        });

        return this;
    }

    /**
     * Prepare the landings in the controller
     * @param {any} tabs
     * @param {any} pages
     */
    prepareLandings(tabs, pages) {
        this.tabs = tabs;
        this.pages = pages;

        this.mainNav.fillTabs(tabs);
        $_.appendChildren(this.swipeBase, ...pages.map(p => p.container));

        //Listen to swipe and navigation events
        $_.addEvents(this.swipeBase, {
            swipemove: e => {
                if (e.target !== this.swipeBase || e.detail.index < 0)
                    return;
                this.moveLanding(e.detail.index, 0);
            }
        });
        $_.addEvents(this.mainNav, {
            tabbarchange: e => this.moveLanding(e.detail.index, 0)
        });

        //Timeout needed here because of delay in construction of custom elements
        //This just moves the page to the appropriate place at startup
        setTimeout(() => {
            this.moveLanding(this.UrlHandler.landing, this.UrlHandler.page, true);
        }, 300);

        return this;
    }

    /**
     * Move the landing to a new position
     * @param {number} landing Landing Index to move to
     * @param {number} page Page Index to move to
     * @param {Boolean} [force] Whether to force the update
     */
    moveLanding(landing, page, force = false) {
        if ((this.swipeBase === null && this.mainNav === null) || (this.tabs.length < 1))
            return;

        let uselanding = landing;
        if (landing > this.tabs.length - 1)
            uselanding = this.tabs.length - 1;

        if (this.UrlHandler.landing !== uselanding) {
            window.history.pushState({
                landing: uselanding,
                page: page
            },
                `Derek Honeycutt : ${this.tabs[uselanding].label}`,
                `?landing=${uselanding}&page=${page}`);
        }
        window.document.title = `Derek Honeycutt : ${this.tabs[uselanding].label}`;

        if (this.UrlHandler.landing !== uselanding || force) {
            this.UrlHandler.landing = uselanding;
            if (this.swipeBase)
                this.swipeBase.moveToIndex(uselanding);
            if (this.mainNav)
                this.mainNav.moveToTabIndex(uselanding);
        }
    }
}
