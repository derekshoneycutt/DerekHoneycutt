import { Imogene as $, ImogeneExports as $_, ImogeneTemplate as $t } from './Imogene/Imogene';
import DrockSwiperElement from './cmp/swiper';
import DrockFab from './cmp/fab';
import DrockTopBar from './cmp/drock/topbar';
import showdown from 'showdown';
import DOMPurify from 'dompurify';
import './ServerTypeDefs';


// Make all purified links open in a new window
DOMPurify.addHook('afterSanitizeAttributes', function (node) {
    // set all elements owning target to target=_blank
    if ('target' in node) {
        node.setAttribute('target', '_blank');
        node.setAttribute('rel', 'noopener');
    }
});

//Setup a URL Handler to deal with URL changes for navigation
let params = new URLSearchParams(document.location.search.substring(1));
var UrlHandler = {
    landing: parseInt(params.get("landing"), 0) || 0,
    startLanding: parseInt(params.get("landing"), 0) || 0,
    page: parseInt(params.get("page"), 0) || 0,
    startPage: parseInt(params.get("page"), 0) || 0
};

/**
 * Move the landing to a new position
 * @param {any} tabs Array of tabs currently shown
 * @param {number} landing Landing Index to move to
 * @param {number} page Page Index to move to
 * @param {DrockSwiperElement} swipeBase Swiping Base element to manipulate
 * @param {DrockTopBar} mainNav Main Navigation Top Bar Element
 * @param {Boolean} [force] Whether to force the update
 */
function moveLanding(tabs, landing, page, swipeBase, mainNav, force = false) {
    if (UrlHandler.landing !== landing) {
        window.history.pushState({
            landing: landing,
            page: page
        },
            `Derek Honeycutt: ${tabs[landing].title}`,
            `?landing=${landing}&page=${page}`);
    }

    if (UrlHandler.landing !== landing || force) {
        UrlHandler.landing = landing;
        if (swipeBase)
            swipeBase.moveToIndex(landing);
        if (mainNav)
            mainNav.moveToTabIndex(landing);
    }
}

/**
 * Construct the home page based on fetched landings
 * @param {Array} landings landings to show on the homepage
 */
function constructHomePage(landings) {
    let homeswiper;
    const homecontainer = $(['div', { class: 'landing-div' },
        homeswiper = $(['drock-swiper',
            {
                class: 'landing-swipe',
                orientation: 'y',
                hidexmove: true
            },
            ['div', { class: 'home-page' },
                ['ul', { class: 'home-page-list' },
                    ...landings.map(l => {
                        const ret = $(['li', { class: 'home-page-list-item' },
                            ['a', { class: 'home-page-list-link' },
                                ['div', { class: 'home-page-icons' },
                                    ['span', { class: 'home-page-icon-actual material-icons' }, l.icon]
                                ],
                                ['div', { class: 'home-page-listing' },
                                    ['div', { class: 'home-page-listing-title' }, l.title],
                                    ['div', { class: 'home-page-listing-subtitle' }, l.subtitle]
                                ]
                            ]
                        ]);
                        return ret;
                    })
                ]
            ]
        ])
    ]);

    return {
        container: homecontainer,
        swiper: homeswiper
    };
}

/**
 * Construct a landing, including its pages
 * @param {any} landing
 */
function constructLanding(landing) {
    const container = $(['div', { class: 'landing-div' }]);
    const swiper = $(['drock-swiper', {
        class: 'landing-swipe',
        orientation: 'y',
        hidexmove: true
    }]);
    container.append(swiper);

    landing.pages.forEach(p => {
        const div = $(['div']);
        const markdown = `# ${p.title}\n\n${p.subtitle}\n\n${p.text || '--'}`;
        const html = DOMPurify.sanitize(new showdown.Converter().makeHtml(markdown));
        div.innerHTML = html;
        swiper.append(div);
    });

    return {
        container: container,
        swiper: swiper
    };
}

/** Fetch the Home object from the server, which tells us everything to build the initial site! */
async function fetchhome() {
    let homefetch = await $_.RestFetch("/", "portfolio");

    /** @type {DrockSwiperElement} */
    const swipeBase = $("#swipe-base")[0];
    /** @type {DrockTopBar} */
    const mainNav = $("#drock-main-nav")[0];

    if (homefetch.landings && homefetch.landings instanceof Array) {
        //Fill the top bar
        const tabs = [
            { icon: 'home', label: 'Home', active: true },
            ...homefetch.landings
                .map(l => ({ icon: l.icon, label: l.title, active: false, order: l.order }))
                .sort((a, b) => (a.order || 0) - (b.order || 0))
        ];
        mainNav.fillTabs(tabs);

        //create the pages
        const pages = [
            constructHomePage(homefetch.landings),
            ...homefetch.landings.map(constructLanding)
        ];
        $_.appendChildren(swipeBase, ...pages.map(p => p.container));

        $_.addEvents(swipeBase, {
            swipemove: e => {
                if (e.target !== swipeBase || e.detail.index < 0)
                    return;
                moveLanding(tabs, e.detail.index, 0, null, mainNav);
            }
        });
        $_.addEvents(mainNav, {
            tabbarchange: e => moveLanding(tabs, e.detail.index, 0, swipeBase, null)
        });

        if (UrlHandler.landing !== 0 || UrlHandler.page !== 0) {
            setTimeout(() => {
                moveLanding(tabs, UrlHandler.landing, UrlHandler.page, swipeBase, mainNav, true);
            }, 300);
        }

        window.onpopstate = event => {
            if (event.state) {
                UrlHandler.landing = event.state.landing;
                UrlHandler.page = event.state.page;
            }
            else {
                UrlHandler.landing = UrlHandler.startLanding;
                UrlHandler.page = UrlHandler.startPage;
            }

            moveLanding(tabs, UrlHandler.landing, UrlHandler.page, swipeBase, mainNav, true);
        };
    }
}

$(() => {

    $('#drock-contactfab').addEvents({
        click: () => console.log('Clicked contact!')
    });

    fetchhome();
});


