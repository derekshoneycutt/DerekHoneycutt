import { Imogene as $, ImogeneExports as $_, ImogeneTemplate as $t } from '../Imogene/Imogene';
import SawBaseElement from './cmp/sawbase';
import SawWeekElement from './cmp/Views/week';
import DrockSplashScreen from '../cmp/splashscreen';
import DrockSwiperElement from '../cmp/swiper';
import { getDummyHome } from './DummyData';

/**
 * Create a new week component
 * @param {number} days number of days to show in the week
 * @returns {SawWeekElement} a new week component
 */
function makeweek(days) {
    let item = $(['saw-week']);
    if (days)
        $_.setProperties(item, { days: days });

    let home = getDummyHome();
    let budget = home.budgets[0].getBudget();
    let events;
    if (days === 1)
        events = budget.getDayView(Date.today());
    else if (days === 3 || days === undefined)
        events = budget.getThreeDayView(Date.today());
    else if (days === 7)
        events = budget.getWeekView(Date.today());
    events = events.sort((a, b) => {
        return a.primaryDate - b.primaryDate;
    });
    item.prepareRows(3, events);

    events.forEach((evt, index) => {
        let evtDiv = $(['div', {
                class: 'saw-allocation',
                slot: `alloc${index + 1}`
            },
            evt.title 
        ]);
        item.append(evtDiv);
    });

    return item;
}

$(() => {
    /** @type {DrockSplashScreen[]} */
    //const splash = $('drock-splash');

    /** @type {SawBaseElement[]} */
    const appbase = $('saw-base');
    $_.addEvents(appbase, {
        addplan: e => console.log('Add plan!'),
        addreceipt: e => console.log('Add receipt!'),
        addnote: e => console.log('Add note!'),

        changeview: e => console.log(`Change view: ${e.detail.view}`),
        expanded: e => console.log(`Expanded? ${e.detail.expanded}`)
    });

    const swipewindow = $('#saw-swipewindow');
    $_.appendChildren(swipewindow,
        makeweek(1),
        makeweek(1),
        makeweek(),
        makeweek(3),
        makeweek(3),
        makeweek(7),
        makeweek(7)
    );

    //splash[0].pauseAndFade();
});