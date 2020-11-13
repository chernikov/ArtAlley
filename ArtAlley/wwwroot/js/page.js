// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// player

function Page() {
    var _this = this;

    var currentMusic = null;
    var currentId = null; 

    this.init = function () {
        $(".play-btn").on("click", function () {
            var music = $(this).siblings("audio");
            music.on({
                loadeddata: function () {
                    console.log("audio is loaded");
                },
                timeupdate: function () {                 
                    updateTimeline(this);
                }
            });
            handlePlay(music);
        });
    }

    handlePlay = function (music) {
        var id = music.data("id");
        if ($("#play_" + id).data("disabled")) {
            return;
        }
        if (music[0].paused) {
            playMusic(music);
        } else {
            stopMusic(music);
        }
    }

    function stopMusic(music) {
        var id = music.data("id");
        var playBtn = $("#play_" + id);
        music[0].pause();
        playBtn.removeClass('pause');
        playBtn.html(`<i class="material-icons">play_arrow</i>`);
        currentMusic = null;
        currentId = null;
    }

    function playMusic(music) {
        if (currentId !== null) {
            stopMusic(currentMusic);
        }
        var id = music.data("id");
        var playBtn = $("#play_" + id);
        music[0].play();
        $.get("/api/play/" + id, function (data) {
            console.log("Logged", id);
        });
        playBtn.addClass('pause');
        playBtn.html('<i class="material-icons">pause</i>');
        currentMusic = music;
        currentId = parseInt(id);
    }

    updateTimeline = function (music) {
        let duration = $(music)[0].duration;
        let currentTime = $(music)[0].currentTime;
        $(music).siblings('.progress').css('width', (currentTime * 100 / duration) + '%');
    }
}

var page = null;
$(_ => {
    page = new Page();
    page.init();
});