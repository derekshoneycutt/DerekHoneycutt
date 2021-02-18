import { Imogene as $, ImogeneExports as $_, ImogeneTemplate as $t } from './Imogene/Imogene';
import DrockSwiperElement from './cmp/swiper';
import DrockFab from './cmp/fab';
import DrockTopBar from './cmp/drock/topbar';
import DrockContact from './cmp/drock/contact';
import DrockMainController from './MainController';
import { MDCRipple } from '@material/ripple';
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


/**
 * Construct the home page based on fetched landings
 * @param {Array} landings landings to show on the homepage
 */
function constructHomePage(landings, onlandingclick) {
    let homeswiper;
    const homecontainer = $(['div', { class: 'landing-div' },
        homeswiper = $(['drock-swiper',
            {
                class: 'landing-swipe',
                orientation: 'y',
                hidexmove: true
            },
            ['div', { class: 'home-page drock-page-base' },
                ['div', { class: 'drock-page-content' },
                    ['div', { class: 'drock-page-background' }],
                    ['div',  { class: 'home-page-list' },
                        ...landings.map((l, index) => {
                            let link;
                            const ret = $(['div', { class: 'home-page-list-item' },
                                link = $(['a', {
                                    class: 'home-page-list-link mdc-ripple-surface',
                                    href: `?landing=${index + 1}&page=0`,
                                    on: {
                                        click: e => {
                                            if (onlandingclick) {
                                                e.preventDefault();
                                                onlandingclick(index, e);
                                            }
                                        }
                                    }
                                },
                                    ['div', { class: 'home-page-icons' },
                                        ['span', { class: 'home-page-icon-actual material-icons' }, l.icon]
                                    ],
                                    ['div', { class: 'home-page-listing' },
                                        ['div', { class: 'home-page-listing-title' }, l.title],
                                        ['div', { class: 'home-page-listing-subtitle' }, l.subtitle]
                                    ]
                                ])
                            ]);
                            link.mdcRipple = new MDCRipple(link);
                            return ret;
                        })
                    ]   
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
        let contentDiv;
        const div = $(['div', { class: 'drock-page-base' },
            contentDiv = $(['div', { class: 'drock-page-content' }])
        ]);
        const markdown = `# ${p.title}\n\n${p.subtitle}\n\n${p.text || '--'}`;
        const html = DOMPurify.sanitize(new showdown.Converter().makeHtml(markdown));
        contentDiv.innerHTML = html;
        swiper.append(div);
    });

    return {
        container: container,
        swiper: swiper
    };
}

/**
 * Fill the landings
 * @param {DrockMainController} controller
 */
function fillLandings(controller) {
    //Fill the top bar
    const tabs = [
        { icon: 'home', label: 'Home', active: true },
        ...controller.homefetch.landings
            .map(l => ({ icon: l.icon, label: l.title, active: false, order: l.order }))
            .sort((a, b) => (a.order || 0) - (b.order || 0))
    ];

    //create the pages
    const pages = [
        constructHomePage(controller.homefetch.landings,
            (index, e) => controller.moveLanding(index + 1, 0, true)),
        ...controller.homefetch.landings.map(constructLanding)
    ];

    controller.prepareLandings(tabs, pages);
}

/** Fetch the Home object from the server, which tells us everything to build the initial site!
 * @param {DrockMainController} controller */
async function fetchhome(controller) {
    let homefetch = await $_.RestFetch("/", "portfolio");

    controller.homefetch = homefetch;

    if (homefetch.landings && homefetch.landings instanceof Array) {
        fillLandings(controller);
    }
}

$(() => {
    const controller = new DrockMainController();
    controller.findElements();
    controller.watchUrl();

    fetchhome(controller);
});
