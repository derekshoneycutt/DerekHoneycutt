import { Imogene as $, ImogeneExports as $_, ImogeneTemplate as $t } from './Imogene/Imogene';
import showdown from 'showdown';
import DOMPurify from 'dompurify';
import { MDCRipple } from '@material/ripple';
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
export function constructHomePage(landings, onlandingclick) {
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
                    ['div', { class: 'home-page-list' },
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
 * Create a Resume Head Page
 * @param {DrockServerPage} page The server-page to construct the HTML from
 * @param {HTMLDivElement} contentDiv content div to add the page to
 */
function createResumeHeadPage(page, contentDiv) {
    let dataDiv = $(['div', { class: 'drock-resumehead-data' }]);
    $_.appendChildren(dataDiv,
        ['div', { class: 'drock-resumehead-body' },
            ['div', { class: 'drock-resumehead-title' }, page.title],
            ['div', { class: 'drock-resumehead-desc' }, page.description]
        ]
    );
    contentDiv.append(dataDiv);
}

/**
 * Create a Resume GitHub Page
 * @param {DrockServerPage} page The server-page to construct the HTML from
 * @param {HTMLDivElement} contentDiv content div to add the page to
 */
function createGitHubPage(page, contentDiv) {
    contentDiv.append(
        $(['div', { class: 'drock-github-data' },
            ['div', { class: 'drock-github-body' },
                ['div', { class: 'drock-github-head' }, 'GitHub'],
                (() => {
                    let githubCard = $(['div', { class: 'mdc-card drock-github-card' },
                        ['a', {
                            class: 'mdc-card__primary-action drock-github-content',
                            tabindex: 0,
                            href: `https://github.com/${page.gitHub}`,
                            target: '_blank'
                        },
                            ['div', { class: 'drock-github-card-img' },
                                ['img', { src: 'GitHub-Mark-120px-plus.png' }]
                            ],
                            ['div', { class: 'drock-github-card-title' }, page.gitHub],
                            ['div', { class: 'drock-github-card-desc' }, page.description]
                        ]
                    ]);
                    githubCard.mdcRipple = new MDCRipple(githubCard);
                    return githubCard;
                })()
            ]
        ]));
}

/**
 * Create a Resume Experience Page
 * @param {DrockServerPage} page The server-page to construct the HTML from
 * @param {HTMLDivElement} contentDiv content div to add the page to
 */
function createResumeExpPage(page, contentDiv) {
    let dataDiv = $(['div', { class: 'drock-resumeexp-data' },
        ['div', { class: 'drock-resumeexp-body' },
            ['div', { class: 'drock-resumeexp-title' }, page.title],
            ['div', { class: 'drock-resumeexp-jobs' },
                ...page.jobs.map(j => {
                    return ['div', { class: 'drock-resumeexp-job' },
                        ['div', { class: 'drock-resumeexp-job-title' }, `${j.employer} : ${j.title}`],
                        ['div', { class: 'drock-resumeexp-job-dates' }, `${j.startDate} - ${j.endDate}`]
                    ];
                })
            ]
        ]
    ]);
    contentDiv.append(dataDiv);
}

/**
 * Construct a landing, including its pages
 * @param {any} landing
 */
export function constructLanding(landing) {
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
            contentDiv = $(['div', { class: `drock-page-content drock-page-${p.type}` }])
        ]);
        if (p.background) {
            $_.setStyle(contentDiv, {
                'background-color': p.background
            });
        }
        if (p.image) {
            const bgDiv = $(['div', { class: 'drock-page-background' }]);
            $_.setStyle(bgDiv, {
                'background-image': `url("${p.image}")`
            });
            contentDiv.append(bgDiv);
        }
        let markdown;
        if (p.type === 'resumehead') {
            createResumeHeadPage(p, contentDiv);
        }
        else if (p.type === 'github') {
            createGitHubPage(p, contentDiv);
        }
        else if (p.type === 'resumeexp') {
            createResumeExpPage(p, contentDiv);
        }
        else {
            markdown = `# ${p.title}\n\n${p.subtitle}\n\n${p.text || p.description || '--'}`;
            const html = DOMPurify.sanitize(new showdown.Converter().makeHtml(markdown));
            contentDiv.innerHTML = html;
        }
        swiper.append(div);
    });

    return {
        container: container,
        swiper: swiper
    };
}