var es = require('event-stream');
var gulp = require('gulp');
var concat = require('gulp-concat');
var gutil = require('gulp-util');
var ftp = require('vinyl-ftp');
//var connect = require('gulp-connect');
var replace = require('gulp-replace');
var browserSync     = require('browser-sync').create();
var templateCache = require('gulp-angular-templatecache');
var ngAnnotate = require('gulp-ng-annotate');
var uglify = require('gulp-uglify');
var fs = require('fs');
const zip = require('gulp-zip');
var _ = require('lodash');
var reload      = browserSync.reload;
var scripts = require('./app.scripts.json');

var source = {
    js: {
        main: 'app/main.js',
        src: [
            // application config
            'app.config.js',

            // application bootstrap file
            'app/main.js',

            // main module
            'app/app.js',

            // module files
            'app/**/module.js',

            // other js files [controllers, services, etc.]
            'app/**/!(module)*.js'
        ],
        tpl: 'app/**/*.tpl.html'
    }
};
//
var destinations = {
    js: 'build',
    deploy : 'deploy'
};
//
gulp.task('build', function(){
    return es.merge(gulp.src(source.js.src) , getTemplateStream())
         .pipe(ngAnnotate())
         .pipe(uglify())
         .pipe(replace('http://10.1.1.194/servicodireto/api', 'http://api.servicodireto.com/api'))
         .pipe(concat('app.js'))
         .pipe(gulp.dest(destinations.js));
});
//
gulp.task('zip', () =>
gulp.src(destinations.deploy + '/**/*')
    .pipe(zip('archive.zip'))
    .pipe(gulp.dest(destinations.deploy))
);
//
gulp.task( 'ftp', function () { 
       var conn = ftp.create( {
           host:     'ftp.site4now.net',
           user:     'igorfranco-001',
           password: 'inter007',
           parallel: 10,
           log:      gutil.log
       } );
    //
       var globs = [ destinations.deploy + '/archive.zip' ];
    
       // using base = '.' will transfer everything to /public_html correctly 
       // turn off buffering in gulp.src for best performance 
       return gulp.src( globs, { base: '.', buffer: false } )
           .pipe( conn.newer( '/ui/admin' ) ) // only upload newer files 
           .pipe( conn.dest( '/ui/admin' ) );
    
   } );
//
gulp.task('deploy', function(){
    gulp.src(['build/**/*']).pipe(gulp.dest(destinations.deploy + '/build'));    
    gulp.src(['app/**/*']).pipe(gulp.dest(destinations.deploy + '/app'));    
    gulp.src(['api/**/*']).pipe(gulp.dest(destinations.deploy + '/api'));  
    gulp.src(['sound/**/*']).pipe(gulp.dest(destinations.deploy + '/sound'));   
    gulp.src(['styles/**/*']).pipe(gulp.dest(destinations.deploy + '/styles')); 
    gulp.src(['index.html'])
    .pipe(replace('dev_app_compiled', 'app_compiled'))
    .pipe(gulp.dest(destinations.deploy))
});
//
gulp.task('js', function(){
    return es.merge(gulp.src(source.js.src) , getTemplateStream())
        .pipe(concat('app.js'))
        .pipe(gulp.dest(destinations.js));
});
//
gulp.task('watch', function(){
    gulp.watch(source.js.src, ['js']);
    gulp.watch(source.js.tpl, ['js']);
});

// Static server
gulp.task('browser-sync', function() {
    browserSync.init({
        port:8888,
        server: 
        {
            directory: false,
            baseDir: "./",
            routes: {
              "/bower_components": "./bower_components/"
            }
        }
    });
});

// Static server
gulp.task('browser-sync-prd', function() {
    browserSync.init({
        port:8887,
        server: 
        {
            directory: false,
            baseDir: "./deploy",
            routes: {
              "/bower_components": "./bower_components/"
            }
        }
    });
});


gulp.task('vendor', function(){
    _.forIn(scripts.chunks, function(chunkScripts, chunkName){
        var paths = [];
        chunkScripts.forEach(function(script){
            var scriptFileName = scripts.paths[script];
            if (!fs.existsSync(__dirname + '/' + scriptFileName)) {
                throw console.error('Required path doesn\'t exist: ' + __dirname + '/' + scriptFileName, script)
            }
            paths.push(scriptFileName);
            console.log(scriptFileName);
        });
        gulp.src(paths)
            .pipe(concat(chunkName + '.js'))
            //.on('error', swallowError)
            .pipe(gulp.dest(destinations.js))
    })

});

gulp.task('prod', ['vendor', 'js', 'build', 'deploy'/*, 'zip', 'ftp'*/]);
gulp.task('dev', ['vendor', 'js', 'watch', 'browser-sync']);
gulp.task('default', ['dev']);

var swallowError = function(error){
    console.log(error.toString());
    this.emit('end')
};

var getTemplateStream = function () {
    return gulp.src(source.js.tpl)
        .pipe(templateCache({
            root: 'app/',
            module: 'app'
        }))
};