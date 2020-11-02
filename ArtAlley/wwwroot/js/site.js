// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// player
var connection = new signalR.HubConnectionBuilder().withUrl("/controlhub").build();

var musicLines = document.getElementsByClassName('music-element');
var playBtns = document.getElementsByClassName('play-btn');
var currentTimes = document.getElementsByClassName('current-time');
var stateBox = document.getElementById("state");

var currentMusic = null;
var currentId = null;
var uuid = uuidv4();

var state = {};
var aliveTimestamp = 0;
var aliveTimeout = 0;

var audioFiles = [
    "/files/01.mp3",
    "/files/02.mp3",
    "/files/03.mp3",
    "/files/04.mp3",
    "/files/05.mp3",
    "/files/06.mp3",
    "/files/07.mp3",
    "/files/08.mp3",
    "/files/09.mp3"
];

function preloadAudio(url) {
    var audio = new Audio();
    // once this file loads, it will call loadedAudio()
    // the file will be kept by the browser as cache
    audio.addEventListener('canplaythrough', loadedAudio, false);
    audio.src = url;
}

_.map(audio => preloadAudio(audio));


connection.on("UpdateState", function (stateFromServer) {
    state = stateFromServer;
    UpdateState();
});

connection.on("CurrentTime", function (time) {
    //state.currentTrackTime = time;
    if (currentMusic) {
        currentMusic.currentTime = time;
    }
});


connection.on("DiffTime", function (diff) {
    aliveTimeout = new Date().getTime() - aliveTimestamp;
    console.log("Timeout", aliveTimeout);
    console.log(diff);
    if (currentMusic && Math.abs(diff) > 1) {
        currentMusic.currentTime = currentMusic.currentTime + diff;
    }
});


connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});


function handlePlay(num) {
    var music = musicLines[num];
    var elem = document.getElementById("play_" + num);
    if (elem.dataset.disabled === "true")
    {
        return;
    }
    if (music.paused) {
        playMusic(num);
    } else {
        stopMusic(num);
    }
}

function stopMusic(num) {
    var music = musicLines[num];
    var playBtn = playBtns[num];
    music.pause();
    playBtn.classList.remove('pause');
    playBtn.innerHTML = '<i class="material-icons">play_arrow</i>';
    StopLine();
    currentMusic = null;
    currentId = null;
}

function playMusic(num) {
    if (currentId !== null) {
        stopMusic(currentId);
    }
    var music = musicLines[num];
    var playBtn = playBtns[num];
    music.play();
    playBtn.classList.add('pause');
    playBtn.innerHTML = '<i class="material-icons">pause</i>';
    StartLine(num);
    currentMusic = music;
    currentId = num;
}

function StartLine(num) {
    connection.invoke("StartLine", uuid, num).catch(function (err) {
        return console.error(err.toString());
    });
}

function StopLine() {
    connection.invoke("StopLine", uuid).catch(function (err) {
        return console.error(err.toString());
    });
}


function UpdateState() {
    stateBox.innerHTML = "<code>" + JSON.stringify(state) + "</code>";
    _.map(playBtns, btn => {
        var id = parseInt(btn.id.substr("play_".length));
        
        if (id !== currentId) {
            if (state.aliveLines && state.aliveLines.length) {
                var line = state.aliveLines.find(p => p.lineNum === id);
                if (line) {
                    blockPlay(btn, true);
                }else {
                    blockPlay(btn, false);
                }
            } else {
                blockPlay(btn, false);
            }
        }
    })
}

function blockPlay(elem, isBlock) {
    if (isBlock) {
        elem.dataset.disabled = "true";
        elem.classList.add('disabled');
    } else {
        elem.dataset.disabled = "false";
        elem.classList.remove('disabled');
    }
}

function Alive() {
    aliveTimestamp = new Date().getTime();

    var currentTime = 0;
    if (currentMusic != null) {
        currentTime = currentMusic.currentTime;
    }
    connection.invoke("Alive", uuid, currentTime).catch(function (err) {
        return console.error(err.toString());
    });
}

function setCurrentTime(num, music) {
    var currentTime = currentTimes[num];
   currentTime.innerHTML = music.currentTime;
}

setInterval(_ => {
    UpdateState();
    Alive();
}, 1000)

_.map(musicLines, music => {
    var id = parseInt(music.id);
    music.onloadeddata = function () {
        console.log("Duration", music.duration);
    }
    music.ontimeupdate = function () {
       setCurrentTime(id, music);
    };

    music.addEventListener('timeupdate', function () {
       setCurrentTime(id, music);
    }, false);

    music.addEventListener('ended', function () {
        stopMusic(id);
        currentMusic.currentTime = 0;
    });
})

function uuidv4() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}


//// volume
var volBox = document.querySelector('.volume-box')
var volumeRange = document.querySelector('.volume-range')
var volumeDown = document.querySelector('.volume-down')
var volumeUp = document.querySelector('.volume-up')

volumeDown.addEventListener('click', handleVolumeDown);
volumeUp.addEventListener('click', handleVolumeUp);

function handleVolumeDown() {
    volumeRange.value = Number(volumeRange.value) - 20
    currentMusic.volume = volumeRange.value / 100
}
function handleVolumeUp() {
    volumeRange.value = Number(volumeRange.value) + 20
    currentMusic.volume = volumeRange.value / 100
}