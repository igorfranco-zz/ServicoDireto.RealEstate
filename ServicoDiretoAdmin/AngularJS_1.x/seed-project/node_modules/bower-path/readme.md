# bower-path [![NPM version](https://badge.fury.io/js/bower-path.svg)](http://badge.fury.io/js/bower-path)

Quickly resolve the main path of a Bower component.

    $ npm install bower-path

-------------
## Resolving a Bower component's path
bower-path looks inside the component's .bower.json file for the ```main``` attribute.

    var resolve = require('bower-path');
    var path = resolve('jquery');

bower-path takes into consideration .bowerrc config files, and defaults to bower_components.

---------
* See: https://www.npmjs.org/package/bower-path
* See: http://github.com/cobbdb/bower-path
* License: MIT
