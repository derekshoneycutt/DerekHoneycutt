

# Derek Honeycutt

This is a repository for my personal portfolio website and code that I'm willing to show as additional portfolio. Although primarily the codebase for my personal "resume" website, there is some work from other projects included.

The primary project here is an intentionally over-engineered C# Web API RESTful backend with a vanilla Javascript frontend built with web components. 

# Subprojects

### Imogene

In the front_src directory is a Javascript library called Imogene. This is a diverse library built around DOM manipulations and an automatic client for compatible RESTful interfaces (e.g. the backend included in this project).

Suggested use of importing this library may be like this:
```javascript
import { Imogene as $, ImogeneExports as $_, ImogeneTemplate as $t } from 'Imogene';
```

The `$` syntax is remarkable for only kind of following a jQuery type syntax. For example, the following is possible:
```javascript
const checkbox = $("#mycheckbox");
$_.setProperties(arrayOfElements, {
	checked: true
});
```
The `$t` syntax, meanwhile is not largely preferred except to quickly write prototyping code. It is nonetheless included and used. An example:
```javascript
let myval = $_.value("Hello");
const newElement = $(['div', myval]);
const alsoElement = $t`<div>${myval}</div>`;
/* ... later */
myval.set("Goodbye"); /* Updates both elements */
```

The `RestFetch` interface is a wrapper over Javascript's `fetch` that automatically constructs a chain of functions that perform `fetch` based upon hyperlink data. This enables a complete interface to a compatible RESTful API. A compatible API must return JSON that essentially follows this pattern:
```json
{
	...,
	"links": [
		{
			"rel": "This becomes the function name in the JS object",
			"method": "HTTP methods supported, separated by a |",
			"href": "address to query",
			"[postData]": "Any string or object representing a template to send back in POST requests"
		},
		...
	]
}
```
Every link will be translated into a function on the returned object of `RestFetch`, based upon method and rel string. For example, using `RestFetch` may look like this:
```javascript
let data = await $_.RestFetch('/api/', 'portfolio', err_callback);
let settings = await data.getSettings(); 
let postData = Object.assign({}, settings.postAddressPostData);
postData .address = '123 New Address Lane';
await settings.postAddress(postData);
```

### Components

Included in the `front_src/cmp` directory is several web components that are broadly useful. There are several highly specialized components included in this project, but the primary extendable ones are highlighted here.

#### Swiper

This is a component that holds a number of children and enables swiping action upon them. Each child should be held within a `<div>` container for proper function.

The following properties are available:

 - `index` : Sets the child that is currently in view, according to its index, starting at 0. Default value is 0.
 - `hidexmove` : Set to truthy value to hide the hover-over buttons to move in the x axis. Should always be true if orientation is `y`. Default value is falsey.
 - `orientation` : Set to `x` to enable swiping on the x axis, and `y` to enable swiping on the y axis instead. Default value is `x`

Additionally, the following methods are enabled on the swiping element:

 - `moveToIndex(index, animate = true)` : Moves the swiper to a given index
 - `movePrevious()` : Moves the swiper to a lower index by 1.
 - `moveNext()` : Moves the swiper to a higher index by 1.

The following events are raised upon extra actions on the element:

 - `swipemove` : Raised when the swiper has moved to a different index. Event object includes `detail` object with `index` property set to new index that the swiper lands upon.

#### Calendar

This is a basic component that displays and manages a calendar widget. The user is able to navigate month to month and click on a date to select a given date on the calendar.

The following properties are available:

 - `usemonth` : Any date in a given month to be displayed on the calendar.
 - `selection` : The currently selected date on the calendar.

The following events are raised upon actions on the element:

 - `changemonth` : Raised when the viewed month is changed by user or code.
 - `choosedate` : Raised when the selected date is changed by user or code.

### Sawtooth

In the frontend, the sawtooth directory contains a prototype work-in-progress for the frontend of a calendar-based project I have been working with. It is built in here to showcase some of my own code, but it is not more than prototype at this point.


# License

This work is hearby released into the public domain.
