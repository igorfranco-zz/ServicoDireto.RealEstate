var fs = require('fs');

/**
 * Resolve the path of a bower component.
 * @param pkg {String} The bower component name.
 */
module.exports = function (pkg) {
    var path;
    try {
        var rc = fs.readFileSync('.bowerrc', 'utf8');
        path = JSON.parse(rc).directory || 'bower_components';
    } catch (err) {
        path = 'bower_components';
    }

    path += '/' + pkg + '/';
    var config = fs.readFileSync(path + '.bower.json', 'utf8');
    var main = JSON.parse(config).main;
    return path + main;
};
