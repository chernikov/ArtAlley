﻿/// <binding BeforeBuild='sass' />
module.exports = function (grunt) {
    'use strict';

    const sass = require('node-sass');

    // Project configuration.
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),

        // Sass
        sass: {
            options: {
                sourceMap: true, // Create source map
                implementation: sass,
                //outputStyle: 'compressed' // Minify output
            },
            dist: {
                files: [
                    {
                        expand: true, // Recursive
                        cwd: "Styles", // The startup directory
                        src: ["**/*.scss"], // Source files
                        dest: "wwwroot/css", // Destination
                        ext: ".css" // File extension
                    }
                ]
            }
        }
    });

    // Load the plugin
    grunt.loadNpmTasks('grunt-sass');

    // Default task(s).
    grunt.registerTask('default', ['sass']);
};