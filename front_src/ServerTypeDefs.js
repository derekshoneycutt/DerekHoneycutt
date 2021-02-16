
/**
 * @callback RestFetchObjectRefresh
 * @param {any} [body]
 * @returns {Promise<RestFetchObject>}
 */

/**
 * @typedef {Object} DrockServerResumeExpJob
 * @property {string[]} linkNames
 * @property {RestFetchObjectRefresh} [refresh]
 * @property {RestFetchObjectRefresh} [getFull]
 * @property {string} self
 * @property {string} [title]
 * @property {string} [employer]
 * @property {string} [employerCity]
 * @property {string} [startDate]
 * @property {string} [endDate]
 * @property {string} [description]
 */

/**
 * @typedef {Object} DrockServerSchool
 * @property {string[]} linkNames
 * @property {RestFetchObjectRefresh} [refresh]
 * @property {RestFetchObjectRefresh} [getFull]
 * @property {string} self
 * @property {string} [name]
 * @property {string} [city]
 * @property {string} [startDate]
 * @property {string} [endDate]
 * @property {string} [program]
 * @property {string} [gpa]
 * @property {string} [other]
 */

/**
 * @typedef {Object} DrockServerPage
 * @property {string[]} linkNames
 * @property {RestFetchObjectRefresh} [refresh]
 * @property {RestFetchObjectRefresh} [getFull]
 * @property {string} self
 * @property {string} type
 * @property {string} [title]
 * @property {string} [subtitle]
 * @property {string} [background]
 * @property {string} [image]
 * @property {string} [orientation]
 * @property {string} [description]
 * @property {string[]} [images]
 * @property {DrockServerResumeExpJob[]} [jobs]
 * @property {string} [competencies]
 * @property {DrockServerSchool[]} [schools]
 * @property {string} [text]
 */

/**
 * @typedef {Object} DrockServerLanding
 * @property {string[]} linkNames
 * @property {RestFetchObjectRefresh} [refresh]
 * @property {RestFetchObjectRefresh} [getFull]
 * @property {string} self
 * @property {string} [href]
 * @property {string} [title]
 * @property {string} [subtitle]
 * @property {string} [icon]
 * @property {DrockServerPage[]} [page]
 */


/**
 * @typedef {Object} PostContactForm
 * @property {string} from
 * @property {string} return
 * @property {string} message
 */

/**
 * @callback DrockServerPostContact
 * @param {PostContactForm} body The body to send
 * @param {any} [details] Details to add to the fetch data
 * @param {Boolean} [simple] Whether the body data should be sent simply w/o translate to JSON string
 * @param {RestFetchErrorCallback} [overrcallback] Optional overriding error callback
 * @returns {RestFetchObject} a parsed RestFetchObject from a server fetch
 */

/**
 * @typedef {Object} DrockServerHome
 * @property {string[]} linkNames
 * @property {RestFetchObjectRefresh} [refresh]
 * @property {RestFetchObjectRefresh} [getFull]
 * @property {DrockServerLanding[]} landings
 * @property {DrockServerPostContact} postContact
 * @property {string} postContactHref
 * @property {PostContactForm} postContactPostData
 */



