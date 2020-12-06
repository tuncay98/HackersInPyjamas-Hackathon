const print = (msg) => {
  chrome.extension.getBackgroundPage().console.log(msg)
}

function create_UUID(){
  var dt = new Date().getTime();
  var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
      var r = (dt + Math.random()*16)%16 | 0;
      dt = Math.floor(dt/16);
      return (c=='x' ? r :(r&0x3|0x8)).toString(16);
  });
  return uuid;
}

chrome.runtime.onInstalled.addListener(function () {

  chrome.storage.local.set({ userID: create_UUID() });

  chrome.declarativeContent.onPageChanged.removeRules(undefined, function () {
    chrome.declarativeContent.onPageChanged.addRules([{
      conditions: [new chrome.declarativeContent.PageStateMatcher({
        pageUrl: { hostEquals: 'www.facebook.com' },
      })
      ],
      actions: [new chrome.declarativeContent.ShowPageAction()]
    }]);
  });
});

const hubUrl = "https://localhost:5001/flow"
const sendUrl = "https://localhost:5001/recieve"
const reportUrl = "https://localhost:5001/report"

let userId = "";

let connection = new signalR.HubConnectionBuilder()
  .withUrl(hubUrl, { skipNegotiation: true, transport: signalR.HttpTransportType.WebSockets })
  .build();


let start = () => {
  connection
    .start().then(function () {
      print("Conntected")
      chrome.storage.local.get("userID", (items)=>{
        userId = items.userID
      })
    })
    .catch(function (err) {
      setTimeout(() => {
        start()
      }, 1000)
    });
}

connection.on("ReceiveMessage", function (message, sender) {

  chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
    let activeTab = tabs[0];
    chrome.tabs.sendMessage(activeTab.id, { "message": message });
  });
 

});


connection.onclose(start());

start()

// This block is new!
chrome.runtime.onMessage.addListener(
  async function (request, sender, sendResponse) {
    request.Id = userId;
    print(userId)
    if(request.Reporting == true){

      const params = {
        headers: {
            'Content-Type': "application/json;charset=utf-8"
        },
        body: JSON.stringify(request),
        method: "POST"
    };
  
      let req = await fetch(reportUrl, params).then(w=> w.status).then(w=>print(w))

      return;
    }
    const params = {
      headers: {
          'Content-Type': "application/json;charset=utf-8"
      },
      body: JSON.stringify(request),
      method: "POST"
  };

    let req = await fetch(sendUrl, params).then(w=> w.status).then(w=>print(w))

  }
);