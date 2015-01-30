// include plug-ins
var gulp = require('gulp');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var del = require('del');

var config = {
	 //Include all js files but exclude any min.js files
	 src: [
		  'Scripts/modernizr*.js'
		  , 'Scripts/jquery-2.1.3.js'
		  , 'Scripts/jquery.validate.js'
		  , 'Scripts/jqeruy.validate.unobtrusive.js'
		  , '!Scripts/*.min.js'
		  , '!Scripts/bootstrap.js'],
}

// Synchronously delete the output file(s)
gulp.task('clean', function () {
	 del.sync(['Scripts/all.min.js'])
});

// Combine and minify all files from the app folder
gulp.task('scripts', ['clean'], function () {

	 gulp.src(config.src)
	  .pipe(uglify())
	  .pipe(concat('all.min.js'))
	  .pipe(gulp.dest('Scripts/'));
});

//Set a default tasks
gulp.task('default', ['scripts'], function () { });
