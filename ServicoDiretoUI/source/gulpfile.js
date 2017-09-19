var es = require('event-stream');
var gulp = require('gulp');
var concat = require('gulp-concat');
var templateCache = require('gulp-angular-templatecache');
var ngAnnotate = require('gulp-ng-annotate');
var replace = require('gulp-replace');
var uglify = require('gulp-uglify');
var browserSync     = require('browser-sync').create();
var fs = require('fs');
var _ = require('lodash');

var reload      = browserSync.reload;

var scripts = require('./app.scripts.json');

var source = {
    js: {
        main: 'development/main.js',
        src: [
            // application config
            'app.config.js',

            // main module
            'development/app.js',
            'development/config.js',
            'development/states.js',
            'development/controllers.js',
            'development/directives.js',
            // module files
            'development/modules/**/module.js',

            // other js files [controllers, services, etc.]
            'development/modules/**/!(module)*.js'
        ],
        tpl: 'development/**/*.tpl.html'
    }
};

var destinations = {
    deploy: 'deploy',
    dev_build: 'development'
};

gulp.task('build', function()
{
    gulp.src(['bower_components/bootstrap/**/*']).pipe(gulp.dest(destinations.deploy + '/components/bootstrap'));
    gulp.src(['bower_components/bootstrap-fileinput/**/*']).pipe(gulp.dest(destinations.deploy + '/components/bootstrap-fileinput'));
    gulp.src(['bower_components/angular-slider/**/*']).pipe(gulp.dest(destinations.deploy + '/components/angular-slider'));
    gulp.src(['bower_components/angularjs-slider/**/*']).pipe(gulp.dest(destinations.deploy + '/components/angularjs-slider'));
    gulp.src(['bower_components/angular-toastr/**/*']).pipe(gulp.dest(destinations.deploy + '/components/angular-toastr'));
    gulp.src(['bower_components/angular-loading-bar/**/*']).pipe(gulp.dest(destinations.deploy + '/components/angular-loading-bar'));
    gulp.src(['bower_components/angular-advanced-searchbox/**/*']).pipe(gulp.dest(destinations.deploy + '/components/angular-advanced-searchbox'));    
    gulp.src(['bower_components/angular-auto-validate/**/*']).pipe(gulp.dest(destinations.deploy + '/components/angular-auto-validate'));
    
    gulp.src(['development/assets/**/*']).pipe(gulp.dest(destinations.deploy + '/assets'));
    gulp.src(['development/modules/**/*.html']).pipe(gulp.dest(destinations.deploy + '/modules'));
    gulp.src(['development/index.html'])
        .pipe(replace('dev_app_compiled', 'app_compiled'))
        .pipe(replace('/bower_components', 'components'))
        .pipe(gulp.dest(destinations.deploy));

    return es.merge(gulp.src(source.js.src) , getTemplateStream())
         .pipe(ngAnnotate())
         .pipe(uglify())
         .pipe(concat('app_compiled.js'))        
         .pipe(replace('http://localhost:8082', 'http://api.servicodireto.com'))
         .pipe(replace('/bower_components', 'components'))
         .pipe(gulp.dest(destinations.deploy));
});

gulp.task('js', function(){
    return es.merge(gulp.src(source.js.src) , getTemplateStream())
        .pipe(concat('dev_app_compiled.js'))
        .pipe(gulp.dest(destinations.dev_build));
});

gulp.task('watch',['browser-sync'], function(){
    gulp.watch(source.js.src, ['js'], reload);
    gulp.watch(source.js.tpl, ['js'], reload);
});


// Watch files for changes
/*
gulp.task('watch', ['browser-sync'], function() {
    // Watch HTML files
    gulp.watch(src.html, reload);    
    gulp.watch(src.js, reload);
    gulp.watch(src.css, reload);
    gulp.watch(src.modules, reload);
});
*/

gulp.task('vendor', function()
{
    _.forIn(scripts.chunks, function(chunkScripts, chunkName){
        var paths = [];
        chunkScripts.forEach(function(script){
            var scriptFileName = scripts.paths[script];
            console.log(scriptFileName);

            if (!fs.existsSync(__dirname + '/' + scriptFileName)) {
                throw console.error('Required path doesn\'t exist: ' + __dirname + '/' + scriptFileName, script)
            }
            paths.push(scriptFileName);
        });
        console.log("Arquivo: " + destinations.deploy + "/" +  chunkName + '.js');
        gulp.src(paths)
            .pipe(concat(chunkName + '.js'))
            .pipe(gulp.dest(destinations.deploy))
            .pipe(gulp.dest(destinations.dev_build))
    })
});

gulp.task('css', function()
{
    _.forIn(scripts.css, function(chunkScripts, chunkName){
        var paths = [];
        chunkScripts.forEach(function(script){
            var scriptFileName = scripts.paths[script];
            console.log(scriptFileName);

            if (!fs.existsSync(__dirname + '/' + scriptFileName)) {
                throw console.error('Required path doesn\'t exist: ' + __dirname + '/' + scriptFileName, script)
            }
            paths.push(scriptFileName);
        });
        console.log("Arquivo: " + destinations.deploy + "/" +  chunkName + '.css');
        console.log(paths);
        gulp.src(paths)
            .pipe(concat(chunkName + '.css'))
            //.on('error', swallowError)
            .pipe(gulp.dest(destinations.deploy))
    })
});

// Static server
gulp.task('browser-sync', function() {
    browserSync.init({
        port:8000,
        server: 
        {
            directory: false,
            baseDir: "./development",
            routes: {
              "/bower_components": "./bower_components/"
            }
        }
    });
});

gulp.task('prod', ['vendor', 'build' ]);
gulp.task('dev', ['vendor', 'js', 'watch', 'browser-sync']);
gulp.task('default', ['dev']);

var swallowError = function(error){
    console.log(error.toString());
    this.emit('end')
};

var getTemplateStream = function () {
    return gulp.src(source.js.tpl)
        .pipe(templateCache({
            root: 'development/',
            module: 'MainApp'
        }))
};