// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// player

function Index() {
    var _this = this;

    this.connection = new signalR.HubConnectionBuilder().withUrl("/controlhub").build();
    this.uuid;

    this.audioFiles = [
        {
            id: 1,
            name: "ALTO 1",
            path: "/files/01.mp3",
            time: 0,
            voiceTime: 5
        },
        {
            id: 2,
            name: "ALTO 2",
            path: "/files/02.mp3",
            time: 0,
            voiceTime: 5 
        }, {
            id: 3,
            name: "ALTO 3",
            path: "/files/03.mp3",
            time: 0,
            voiceTime: 5
        },
        {
            id: 4,
            name: "BASS 1",
            path: "/files/04.mp3",
            time: 0,
            voiceTime: 16
        }, {
            id: 5,
            name: "BASS 2",
            path: "/files/05.mp3",
            time: 0,
            voiceTime: 16
        },
        {
            id: 6,
            name: "BASS 3",
            path: "/files/06.mp3",
            time: 0,
            voiceTime: 16
        }, {
            id: 7,
            name: "SOPRANO 1",
            path: "/files/07.mp3",
            time: 0,
            voiceTime: 1
        },
        {
            id: 8,
            name: "SOPRANO 2",
            path: "/files/08.mp3",
            time: 0,
            voiceTime: 1
        }, {
            id: 9,
            name: "SOPRANO 3",
            path: "/files/09.mp3",
            time: 0,
            voiceTime: 1
        },
        {
            id: 10,
            name: "TENOR 1",
            path: "/files/10.mp3",
            time: 0,
            voiceTime: 11
        },
        {
            id: 11,
            name: "TENOR 2",
            path: "/files/11.mp3",
            time: 0,
            voiceTime: 11
        }, {
            id: 12,
            name: "TENOR 3",
            path: "/files/12.mp3",
            time: 0,
            voiceTime: 11
        }
    ];
    var duration = 94;
    var currentMusic = null;
    var currentId = null;

    var state = {};
    var loaded = 0;
    var downtimer = 5;
    var hash = null;

    this.preloadAll = function() {
        _this.audioFiles.forEach(music => {
            preloadAudio(music.path);
        });
    }

    this.init = function () {
        _this.uuid = uuidv4();
        initConnection();
        initMusicLines();
       
        setInterval(_ => {
            UpdateState();
            if (_this.connection.state === "Connected") {
                Alive();
            }
        }, 1000)
    }

    function initConnection() {
        _this.connection = new signalR.HubConnectionBuilder().withUrl("/controlhub").build();
        _this.connection.on("UpdateState", function (stateFromServer) {
            state = stateFromServer;
            UpdateState();
        });

        _this.connection.on("CurrentTime", function (time) {
            //state.currentTrackTime = time;
            if (currentMusic) {
                currentMusic.currentTime = time;
            }
        });

        _this.connection.on("DiffTime", function (diff) {
            aliveTimeout = new Date().getTime() - aliveTimestamp;
            if (currentMusic && Math.abs(diff) > 0.2 && downtimer > 0) {
                currentMusic.currentTime = currentMusic.currentTime + diff;
                downtimer--;
            } else {
                downtimer = 5;
            }
        });

        _this.connection.start().then(function () {
            
            Alive();
        }).catch(function (err) {
            return console.error(err.toString());
        });
    }

    function initMusicLines() {
        _this.audioFiles.forEach(item => {
            let musicElement = $(`<audio data-id="${item.id}" > <source src="${item.path}" type="audio/mp3"></audio>`).addClass('music-element');
            musicElement.on({
                loadeddata: function () {
                    console.log("audio is loaded");
                },
                timeupdate: function () {
                    setCurrentTime(item, musicElement[0]);
                },
                ended: function () {
                    stopMusic(musicElement);
                    setCurrentTime(item, musicElement[0]);
                },
                canplaythrough: function () {
                    loaded++;
                    //if (loaded == _this.audioFiles.length) {
                    //    // all have loaded
                    //    _this.init();
                    //}
                }
            });

            let label = $(`<label>${item.name}</label>`);
            let progress = $(`<div class='progress' id="progress_${item.id}"></div>`);
            let voiceBlock = $("<div class='voice'></div>").css("width", `${(duration - item.voiceTime) * 100 / (duration)}%`);

            let playBtn = $(`<span id="play_${item.id}"><i class="material-icons">play_arrow</i></span>`).addClass('play-btn');
            $(playBtn).click(function () {
                handlePlay(musicElement);
            });

            let musicBox = $("<div></div>").addClass("music-box");
            musicBox.append(musicElement, label, playBtn, progress, voiceBlock);
            $(".music-list").append(musicBox);
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
        StopLine();
        currentMusic = null;
        currentId = null;
        downtimer = 5;
    }

    function playMusic(music) {
        if (currentId !== null) {
            return;
        }
        var id = music.data("id");
        var playBtn = $("#play_" + id);
        music[0].play();
        playBtn.addClass('pause');
        playBtn.html('<i class="material-icons">pause</i>');
        StartLine(id);
        currentMusic = music[0];
        currentId = parseInt(id);
    }

    function StartLine(id) {
        _this.connection.invoke("StartLine", _this.uuid, id).catch(function (err) {
            return console.error(err.toString());
        });
    }

    function StopLine() {
        _this.connection.invoke("StopLine", _this.uuid).catch(function (err) {
            return console.error(err.toString());
        });
    }


    function UpdateState() {

        var lines = _.map(state.aliveLines, p => p.userId);
        var newHash = JSON.stringify(lines).split('').reduce((a, b) => { a = ((a << 5) - a) + b.charCodeAt(0); return a & a }, 0);
        $("#state").html(`<code>${JSON.stringify(state)}</code>`);
        if (hash === newHash) {
            return;
        }
        hash = newHash;
        $('.play-btn').each(function () {
            var btn = this;
            let id = parseInt($(btn).attr('id').substr("play_".length));
            if (id !== currentId) {
                if (state.aliveLines && state.aliveLines.length) {
                    var line = state.aliveLines.find(p => p.lineNum === id);
                    if (line) {
                        blockPlay(btn, true);
                        $(btn).addClass('pause');
                        $(btn).html('<i class="material-icons">pause</i>');
                    } else {
                        blockPlay(btn, false);
                        $(btn).removeClass('pause');
                        $(btn).html('<i class="material-icons">play_arrow</i>');
                    }
                } else {
                    blockPlay(btn, false);
                    $(btn).removeClass('pause');
                    $(btn).html('<i class="material-icons">play_arrow</i>');
                }
            }
            if (currentId && currentId !== id) {
                blockPlay(btn, true);
            }
        })
    }

    function blockPlay(elem, isBlock) {
        $(elem).data('disabled', isBlock);
        if (isBlock) {
            $(elem).addClass('disabled');
        } else {
            $(elem).removeClass('disabled');
        }
    }

    function Alive() {
        var currentTime = 0;
        if (currentMusic != null) {
            currentTime = currentMusic.currentTime;
        }
        _this.connection.invoke("Alive", _this.uuid, currentTime).catch(function (err) {
            return console.error(err.toString());
        });
    }

    function setCurrentTime(musicLine, music) {
        var progress = $("#progress_" + musicLine.id);
        if (!currentMusic) {
            progress.css("width", '0');
            return;
        }
        progress.css("width", `${(music.currentTime / duration) * 100}%`);
    }


    function uuidv4() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }



}

var index = null;
$(_ => {
    index = new Index();
    index.init();
});