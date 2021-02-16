import '../DateExts';

function getDummyTotal(id, acct, postalloc, posttrans, pend, state) {
    return {
        self: `dummy_total${id}`,
        account: acct,
        postAllocation: postalloc,
        postTransaction: posttrans,
        pending: pend,
        statement: state
    };
}

const repeat = (length, cb) => Array.from({ length }, cb);

/** @type {Date} */
const dummytoday = Date.today();
var DummyDatabase = {
    currentStart: dummytoday,
    accounts: [
        {
            self: 'dummy_account1',
            title: 'My Bank',
            category: 'Checking',

            totals: []
        }
    ],
    events: [],
    notes: []
};

function insertDummyEvent(event) {
    const acct = DummyDatabase.accounts.findIndex(v => v.title === event.fromAccount);
    if (acct < 0) return;
    const totals = DummyDatabase.accounts[acct].totals;

    const startDate = DummyDatabase.currentStart.compareDate(event.primaryDate);
    const isAlloc = !!event.endDate;
    const endDate = isAlloc ? DummyDatabase.currentStart.compareDate(event.endDate) : undefined;

    if ((!isAlloc && startDate < 0) || (isAlloc && endDate < 0))
        return;

    DummyDatabase.events.push(event);

    for (let i = parseInt(startDate); i < totals.length; ++i) {
        if (isAlloc) {
            totals[i].postAllocation -= event.value;
        }
        else {
            totals[i].postTransaction -= event.value;
            if (event.pending)
                totals[i].pending -= event.pending;
            else
                totals[i].pending -= event.value;
        }
    }
}

function prepareDummyInitialViews(baseOnDate) {
    DummyDatabase.currentStart = baseOnDate;
    DummyDatabase.accounts.forEach(v => {
        v.totals = repeat(180, (_, n) => getDummyTotal(n, DummyDatabase.accounts[0].title, 2000, 2000, 2000));
    });
    DummyDatabase.events = [];
    DummyDatabase.notes = [];


    insertDummyEvent({
        fromAccount: 'My Bank', title: 'Thing 1', value: 20,
        primaryDate: Date.today(), endDate: Date.today()
    });
    insertDummyEvent({
        fromAccount: 'My Bank', title: 'Thing 2', value: 60,
        primaryDate: Date.today(), endDate: Date.today().plusDays(2)
    });
    insertDummyEvent({
        fromAccount: 'My Bank', title: 'Thing 3', value: 40,
        primaryDate: Date.today().plusDays(1), endDate: Date.today().plusDays(2)
    });
    insertDummyEvent({
        fromAccount: 'My Bank', title: 'Thing 4', value: 30,
        primaryDate: Date.today(), endDate: Date.today().plusDays(1)
    });
    insertDummyEvent({
        fromAccount: 'My Bank', title: 'Thing 5', value: 200,
        primaryDate: Date.today().plusDays(-1), endDate: Date.today().plusDays(4)
    });
}
prepareDummyInitialViews(dummytoday.plusDays(-60));

function getDummyBudget(id) {
    return {
        self: `dummy_budget${id}`,

        views: [],
        getDayView: () => { },
        getThreeDayView: () => { },
        getWeekView: () => { },
        getMonthView: () => { },
        getAgendaView: () => { },
        getReportsView: () => { },

        links: [],
        refresh: () => getDummyBudget(id)
    };
}

function getDummyBudgetDefinition(id, title) {
    return {
        self: `dummy_budget${id}`,
        title: title,

        links: [],
        refresh: () => getBudgetDefinition(id, title),
        getBudget: () => getDummyBudget(id)
    };
}

export function getDummyHome() {


    return {
        self: 'dummy_home',
        budgets: [
            getDummyBudgetDefinition('1', 'My Budget')
        ],

        links: [],
        refresh: getDummyHome
    };
}
