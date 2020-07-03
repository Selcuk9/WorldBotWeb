const btnAdd = document.getElementById('btn-add');
const listBots = document.querySelector('.list-bots');
const tokenInput = document.getElementById('token');
const listUl = document.querySelector('.list-bots');
const url = '/Telegram/AddBot';
const urlRun = '/Telegram/RunBots';
const urlRunOne = '/Telegram/RunBot';
const urlStop = '/Telegram/StopBots';
const urlStopOne = '/Telegram/StopBot';
const urlDelete = '/Telegram/DeleteBot';

btnAdd.addEventListener('click', async function () {
    if (tokenInput.value != "" && tokenInput.value != null) {


        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(tokenInput.value)
        }).then(response => response.json());


        listBots.innerHTML += `<li id="${listUl.childNodes.length - 1}" onclick="GetChat(event);">
                                    ${response.username}
                                    <a class="btn-start btn-primary" id="${tokenInput.value}" onclick="GetChat(event);">Запустить</a>
                                    <a class="btn-stop btn-primary" id="${tokenInput.value}" onclick="GetChat(event);">&#10060;</a><br>
                                </li>`
        tokenInput.value = "";
    }
});

function GetChat(event) {
    alert(event.target.id);
}


let btnRun = document.getElementById('btn-runAll'),
    btnStop = document.getElementById('btn-stopAll'),
    btnRunOne = document.querySelector('.btn-start'),
    btnStopOne = document.querySelector('.btn-stop');
// everything bots
btnRun.addEventListener('click', function () {
    fetch(urlRun, {
        method: 'POST'
    });
});
btnStop.addEventListener('click', function () {
    fetch(urlStop, {
        method: 'POST'
    });
})

// Each bot
btnRunOne.addEventListener('click', async function (element) {
    if (btnRunOne.classList.contains('btn-start')) {
        const result = await fetch(urlRunOne, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(element.target.id)
        });
        if (result.ok) {
            btnRunOne.innerHTML = 'Остановить';
            btnRunOne.classList.remove('btn-start');
            btnRunOne.classList.add('btn-stop');
            return;
        }
    }
    if (btnRunOne.classList.contains('btn-stop')) {
        const result = await fetch(urlStopOne, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(element.target.id)
        });

        if (result.ok) {
            btnRunOne.innerHTML = 'Запустить';
            btnRunOne.classList.remove('btn-stop');
            btnRunOne.classList.add('btn-start');
            return;
        }
    }

   
});

//btnStopOne.addEventListener('click', async function (element) {
    
//});
