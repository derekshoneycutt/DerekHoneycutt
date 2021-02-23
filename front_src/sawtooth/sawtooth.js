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
    let item = $t`<saw-week></saw-week>`;
    if (days)
        $_.setProperties(item, { days: days });
    item.prepareRows(3, [
        { primaryDate: Date.today(), endDate: Date.today() },
        { primaryDate: Date.today(), endDate: Date.today().plusDays(2) },
        { primaryDate: Date.today().plusDays(1), endDate: Date.today().plusDays(2) },
        { primaryDate: Date.today(), endDate: Date.today().plusDays(1) },
        { primaryDate: Date.today().plusDays(-1), endDate: Date.today().plusDays(4) }
    ]);
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