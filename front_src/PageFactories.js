import { Imogene as $, ImogeneExports as $_, ImogeneTemplate as $t } from './Imogene/Imogene';
import showdown from 'showdown';
import DOMPurify from 'dompurify';
import { MDCRipple } from '@material/ripple';
import './ServerTypeDefs';
import DrockMainController from './MainController';

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
 * @param {DrockMainController} controller Controller object
 * @param {Array} landings landings to show on the homepage
 * @param {(number, Event) => void} onlandingclick Event to call when a landing is clicked on
 */
export function constructHomePage(controller, landings, onlandingclick) {
    const homecontainer = $(['div', { class: 'landing-div home-landing-div' },
        ['div', { class: 'home-page drock-page-base' },
            ['div', { class: 'drock-page-background' }],
            ['div', { class: 'home-page-list drock-page-data' },
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
    ]);

    return {
        container: homecontainer
    };
}

/**
 * Create a Resume Head Page
 * @param {DrockServerPage} page The server-page to construct the HTML from
 * @param {HTMLDivElement} contentDiv content div to add the page to
 */
function createResumeHeadPage(page, contentDiv) {
    let dataDiv = $(['div', { class: 'drock-page-data drock-resumehead-data' }]);
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
        $(['div', { class: 'drock-page-data drock-github-data' },
            ['div', { class: 'drock-github-body' },
                ['div', { class: 'drock-github-head' }, 'GitHub'],
                (() => {
                    let githubCard = $(['div', { class: 'mdc-card drock-github-card' },
                        ['a', {
                            class: 'mdc-card__primary-action drock-github-content',
                            tabindex: 0,
                            href: `https://github.com/${page.gitHub}`,
                            target: '_blank',
                            rel: 'noopener'
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
    let dataDiv = $(['div', { class: 'drock-page-data drock-resumeexp-data' },
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
 * Create a Text Block Page
 * @param {DrockServerPage} page The server-page to construct the HTML from
 * @param {HTMLDivElement} contentDiv content div to add the page to
 */
function createTextBlockPage(page, contentDiv) {
    let dataDiv = $(['div', { class: 'drock-page-data drock-textblock-data' }]);
    const html = DOMPurify.sanitize(new showdown.Converter().makeHtml(page.text));
    $_.appendChildren(dataDiv,
        ['div', { class: 'drock-textblock-body' },
            ['div', { class: 'drock-textblock-title' }, page.title],
            ['div', {
                class: 'drock-textblock-text',
                innerHTML: html
            }]
        ]
    );
    contentDiv.append(dataDiv);
}

/**
 * Create a Image Wall Page
 * @param {DrockServerPage} page The server-page to construct the HTML from
 * @param {HTMLDivElement} contentDiv content div to add the page to
 */
function createImageWallPage(page, contentDiv, controller, landingIndex) {
    let dataDiv = $(['div', { class: 'drock-page-data drock-imagewall-data' }]);
    const html = DOMPurify.sanitize(new showdown.Converter().makeHtml(page.description));
    $_.appendChildren(dataDiv,
        ['div', { class: 'drock-imagewall-body' },
            ['div', { class: 'drock-imagewall-title' }, page.title],
            ['div', {
                class: 'drock-imagewall-text',
                innerHTML: html
            }],
            ['div', { class: 'drock-imagewall-images' },
                ['ul', { class: 'mdc-image-list mdc-image-list--with-text-protection drock-image-list' },
                    ...page.images.map((img, index) => {
                        return ['li', {
                            class: 'mdc-image-list__item drock-image-item',
                            on: {
                                click: e => {
                                    controller.showImgDialog(img.source, landingIndex, index);
                                }
                            }
                        },
                            ['div', { class: 'mdc-image-list__image-aspect-container' },
                                ['img', { class: "mdc-image-list__image", src: img.source }]
                            ],
                            ['div', { class: 'mdc-image-list__supporting' },
                                ['span', { class: 'mdc-image-list__label' }, img.description]
                            ]
                        ];
                    })
                ]
            ]
        ]
    );
    contentDiv.append(dataDiv);
}

/**
 * Create a Schools Page
 * @param {DrockServerPage} page The server-page to construct the HTML from
 * @param {HTMLDivElement} contentDiv content div to add the page to
 */
function createSchoolsPage(page, contentDiv) {
    let dataDiv = $(['div', { class: 'drock-page-data drock-schools-data' },
        ['div', { class: 'drock-schools-body' },
            ['div', { class: 'drock-schools-title' }, page.title],
            ['div', { class: 'drock-schools-schools' },
                ...page.schools.map(j => {
                    return ['div', { class: 'drock-schools-school' },
                        ['div', { class: 'drock-schools-school-title' },
                            `${j.name}${j.city ? `, ${j.city}` : ''}${j.startDate && j.endDate ? `, ${j.startDate} - ${j.endDate}` : ''}`],
                        ['div', { class: 'drock-schools-school-program' }, j.program]
                    ];
                })
            ]
        ]
    ]);
    contentDiv.append(dataDiv);
}

/**
 * Construct a landing, including its pages
 * @param {DrockMainController} controller Controller object
 * @param {number} landingIndex index of the landing being constructed
 * @param {DrockServerLanding} landing landing to construct in html
 */
export function constructLanding(controller, landingIndex, landing) {
    const container = $(['div', { class: 'landing-div' }]);

    const pages = landing.pages.length;
    landing.pages.forEach((p, i) => {
        const zindex = pages - i;
        const div = $(['div', {
            class: 'drock-page-base',
            style: {
                'z-index': zindex
            }
        }]);
        if (p.background || p.image) {
            const bgDiv = $(['div', { class: 'drock-page-background' }]);
            if (p.background) {
                $_.setStyle(bgDiv, {
                    'background-color': p.background,
                    opacity: 1
                });
            }
            if (p.image) {
                $_.setStyle(bgDiv, {
                    'background-image': `url("${p.image}")`,
                    opacity: '0.2'
                });
            }
            div.append(bgDiv);
        }
        let markdown;
        if (p.type === 'resumehead') {
            createResumeHeadPage(p, div);
        }
        else if (p.type === 'github') {
            createGitHubPage(p, div);
        }
        else if (p.type === 'resumeexp') {
            createResumeExpPage(p, div);
        }
        else if (p.type === 'schools') {
            createSchoolsPage(p, div);
        }
        else if (p.type === 'imagewall') {
            createImageWallPage(p, div, controller, landingIndex);
        }
        else if (p.type === 'textblock') {
            createTextBlockPage(p, div);
        }
        else {
            markdown = `# ${p.title}\n\n${p.subtitle}\n\n${p.text || p.description || '--'}`;
            const html = DOMPurify.sanitize(new showdown.Converter().makeHtml(markdown));
            div.innerHTML = html;
        }
        p.pageBaseDiv = div;
        container.append(div);
    });

    if (landing.pages.length > 1) {
        const scrollIndicator = $(['div', { class: 'landing-div-scrollmore' },
            ['span', { class: 'landing-div-scrollmore-icon material-icons' }, 'expand_more'],
            ['span', { class: 'landing-div-scrollmore-desc' }, 'Scroll for more']
        ]);
        container.append(scrollIndicator);
        $_.addEvents(container, {
            scroll: e => {
                $_.setClassList(scrollIndicator, {
                    hidden: (container.scrollTop >= (container.scrollHeight - (container.offsetHeight * 1.75)))
                });
            }
        });
    }

    return {
        container: container
    };
}