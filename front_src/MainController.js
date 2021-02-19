import { Imogene as $, ImogeneExports as $_, ImogeneTemplate as $t } from './Imogene/Imogene';
import { MDCSnackbar } from '@material/snackbar';

/** Main controller class for managing the porfolio interface */
export default class DrockMainController {
    constructor() {
        this._allowContactSend = true;

        this.homefetch;
        this.tabs = [];
        this.pages = [];

        this.swipeBase = null;
        this.mainNav = null;

        this.contactDialogOpen = false;
        this.contactDialog = null;
        this.contactEl = null;
        this.contactFab = null;
        this.contactSnackbar = null;
        this.contactMdcSnackbar = null;

        this.UrlHandler = { landing: 0, page: 0, contact: false };
    }

    /**
     * Event raised when sending email (NOTHING RIGHT NOW!)
     * @param {any} evt
     */
    async onSendEmail(evt) {
        /*if (this._allowContactSend) {
            if (this.homefetch && this.homefetch.postContact) {
                this._allowContactSend = false;
                const contactRet = await this.homefetch.postContact(evt.detail);
                console.log(contactRet);
                this.contactMdcSnackbar.open();
                setTimeout(() => this._allowContactSend = true, 120000)
            }

            $_.setClassList(this.contactDialog, {
                active: false
            });
            this.contactEl.clearFields();
        }
        else {
            alert('Already sent me something. Please wait 2 minutes to send another message.')
        }*/
    }

    /** Watch URL for changes and handle accordingly */
    watchUrl() {
        //Setup a URL Handler to deal with URL changes for navigation
        let params = new URLSearchParams(document.location.search.substring(1));
        this.UrlHandler.landing = parseInt(params.get("landing"), 0) || 0;
        this.UrlHandler.page = parseInt(params.get("page"), 0) || 0;
        this.UrlHandler.contact = !!params.get("contact");
        if (this.UrlHandler.contact) {
            this.launchContactDlg(false);
        }
        else {
            this.closeContactDlg(false);
        }

        //When the history changes (e.g. going back/forward), see if we can just navigate there!
        window.onpopstate = event => {
            if (event.state) {
                this.UrlHandler.landing = event.state.landing;
                this.UrlHandler.page = event.state.page;
                this.UrlHandler.contact = event.state.contact;
            }
            else {
                this.UrlHandler.landing = 0;
                this.UrlHandler.page = 0;
                this.UrlHandler.contact = false;
            }

            this.moveLanding(this.UrlHandler.landing, this.UrlHandler.page, this.UrlHandler.contact, true);

            if (this.UrlHandler.contact) {
                this.launchContactDlg(false);
            }
            else {
                this.closeContactDlg(false);
            }
        };

        return this;
    }

    /**
     * Launch the Contact dialog
     * @param {Boolean} pushHistory whether to push the contact open state in the browser history
     */
    launchContactDlg(pushHistory = true) {
        if (!this.contactDialogOpen) {
            this.contactDialogOpen = true;
            $_.setClassList(this.contactDialog, {
                active: true
            });
            if (pushHistory) {
                window.history.pushState({
                    landing: this.UrlHandler.landing,
                    page: this.UrlHandler.page,
                    contact: true
                },
                    `Derek Honeycutt : ${this.tabs[this.UrlHandler.landing].label}`,
                    `?landing=${this.UrlHandler.landing}&page=${this.UrlHandler.page}&contact=true`);
            }
        }
    }

    /**
     * Close the Contact dialog, if it is not open
     * @param {Boolean} popHistory Whether to pop the latest history state in the browser
     */
    closeContactDlg(popHistory = true) {
        if (this.contactDialogOpen) {
            this.contactDialogOpen = false;
            $_.setClassList(this.contactDialog, {
                active: false
            });
            this.contactEl.clearFields();
            if (popHistory)
                window.history.back();
        }
    }

    /** Fill the elements in the controller based on those in the document */
    findElements() {
        this.swipeBase = $("#swipe-base")[0];
        this.mainNav = $("#drock-main-nav")[0];
        this.contactDialog = $("#drock-contact-dialog")[0];
        this.contactEl = $("#drock-contact-dlg")[0];
        this.contactFab = $('#drock-contactfab')[0];
        this.contactSnackbar = $("#drock-contact-snackbar")[0];
        this.contactMdcSnackbar = new MDCSnackbar(this.contactSnackbar);

        $_.addEvents(this.contactDialog, {
            requestclose: e => {
                this.closeContactDlg();
            },
            send: e => this.onSendEmail(e)
        });

        $_.addEvents(this.contactFab, {
            click: () => {
                this.launchContactDlg();
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
                this.moveLanding(e.detail.index, 0, false);
            }
        });
        $_.addEvents(this.mainNav, {
            tabbarchange: e => this.moveLanding(e.detail.index, 0, false)
        });

        //Timeout needed here because of delay in construction of custom elements
        //This just moves the page to the appropriate place at startup
        setTimeout(() => {
            this.moveLanding(this.UrlHandler.landing, this.UrlHandler.page, this.UrlHandler.contact, true);
        }, 300);

        return this;
    }

    /**
     * Move the landing to a new position
     * @param {number} landing Landing Index to move to
     * @param {number} page Page Index to move to
     * @param {Boolean} contact Whether the contact dialog is shown
     * @param {Boolean} [force] Whether to force the update
     */
    moveLanding(landing, page, contact, force = false) {
        if ((this.swipeBase === null && this.mainNav === null) || (this.tabs.length < 1))
            return;

        let uselanding = landing;
        if (landing > this.tabs.length - 1)
            uselanding = this.tabs.length - 1;

        if (this.UrlHandler.landing !== uselanding) {
            window.history.replaceState({
                landing: uselanding,
                page: page,
                contact: contact
            },
                `Derek Honeycutt : ${this.tabs[uselanding].label}`,
                `?landing=${uselanding}&page=${page}${contact ? '&contact=true' : ''}`);
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
