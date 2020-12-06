chrome.runtime.onMessage.addListener(
  function (request, sender, sendResponse) {
    console.log(request)
    console.log(posts[request.message.index])
    if(!posts[request.message.index].querySelector(".reportClass")){
      appendReport(posts[request.message.index])
      appendRelabilityBtn(posts[request.message.index])
      appendSrc(posts[request.message.index])
    }
    posts[request.message.index].querySelector(".reportClass").setAttribute('title', request.message.reportAmount)
    if(request.message.isReported == true) {
      posts[request.message.index].querySelector(".reportClass").style.backgroundColor = "green";
      posts[request.message.index].querySelector(".reportClass").innerText = "Reported"
    }
    posts[request.message.index].querySelector(".reliabilityID").innerText = request.message.percentage + "%"
  }
);

const delay = millis => new Promise((resolve, reject) => {
  setTimeout(_ => resolve(), millis)
});

let posts = [];

const reportCss =
  `    
position: absolute;
width: 60px;
height: 26px;
background: rgb(236, 92, 92);
color: white;
right: -11.5%;
margin-top: 35px;
display: flex;
justify-content: center;
border-radius: 7px;
align-items: center;
cursor: pointer;
`

const realibilityCss =
  `
  right: -4%;
  margin-top: -23px;
  position: absolute;
  height: 40px;
  width: 40px;
  background-color: rgb(236, 92, 92);
  border-radius: 50%;
  display: flex;
  color: rgb(255, 244, 244);
  justify-content: center;
  align-items: center;
  z-index: 5;
`

const sourceCss =
  `
  position: absolute;
  width: 60px;
  height: 26px;
  background: rgb(236, 92, 92);
  color: white;
  right: -11.5%;
  margin-top: 70px;
  /* top: 15%; */
  display: flex;
  justify-content: center;
  border-radius: 7px;
  align-items: center;
  cursor: pointer;
`

const dropdownHideCss =
  `
display: none;
position: absolute  
`
const dropdownShowCss =
  `
display: block;
`



let appendReport = (element) => {

  let titleElement = element.querySelector(".oajrlxb2.g5ia77u1.qu0x051f.esr5mh6w.e9989ue4.r7d6kgcz.rq0escxv.nhd2j8a9.nc684nl6.p7hjln8o.kvgmc6g5.cxmmr5t8.oygrvhab.hcukyx3x.jb3vyjys.rz4wbd8a.qt6c0cv9.a8nywdso.i1ao9s8h.esuyzwwr.f1sip0of.lzcic4wl.oo9gr5id.gpro0wi8")
  let descriptionElement = element.querySelector(".d2edcug0.hpfvmrgz.qv66sw1b.c1et5uql.rrkovp55.a8c37x1j.keod5gw0.nxhoafnm.aigsh9s9.d3f4x2em.fe6kdd0r.mau55g9w.c8b282yb.iv3no6db.jq4qci2q.a3bd9o3v.knj5qynh.oo9gr5id.hzawbc8m");

  let desc = !!descriptionElement ? descriptionElement.innerText : "No Description"
  let title = !!titleElement ? titleElement.href.split('?')[0] : "Group Name"

  let reportBtn = document.createElement("div");

  element.appendChild(reportBtn)


  reportBtn.style = reportCss;
  reportBtn.innerText = "Report"

  reportBtn.className = "reportClass"

  reportBtn.addEventListener("click", (e)=>{
    e.target.style.backgroundColor = "green";
    e.target.innerText = "Reported"
    e.target.setAttribute('title', 'Already reported')

    chrome.runtime.sendMessage({Title: title, Description: desc, Index: posts.indexOf(element), Reporting: true})
  })


    reportBtn.setAttribute('title', 'Report as fake')
  
}


let appendRelabilityBtn = (element) => {

  let relabilityBtn = document.createElement("div")
  let percetangeSpan = document.createElement("span")

  element.appendChild(relabilityBtn)
  relabilityBtn.appendChild(percetangeSpan)

  relabilityBtn.style = realibilityCss
  percetangeSpan.innerText = "80%"

  relabilityBtn.className = "reliabilityID"

  relabilityBtn.setAttribute('title', 'A reliability percentage')
  

}

let srcCss = `
position: absolute;
bottom: 0;
height: 100px;
width: 150px;
background: red;
`

let appendSrc = (element) => {

  let sourceBtn = document.createElement("div")
  let hov = document.createElement("div")
  let srcDiv = document.createElement("div")


  element.appendChild(sourceBtn)
  sourceBtn.appendChild(hov);
  hov.appendChild(srcDiv)

  
  sourceBtn.style = sourceCss
  sourceBtn.innerText = "Source"

  hov.style.position = "realtive"
  hov.style.width = "200px"
  hov.style.height = "150px" 


  srcDiv.style = srcCss


}


let set = async () => {
  await delay(2000)
  //let targetNode = document.querySelectorAll('[role="feed"]')[document.querySelectorAll('[role="feed"]').length-1];

  let targetNode = document.querySelectorAll('[role="feed"]')[document.querySelectorAll('[role="feed"]').length - 1];

  const callback = function (mutationsList, observer) {
    // Use traditional 'for loops' for IE 11
    for (const mutation of mutationsList) {
      mutation.addedNodes.forEach(element => {
        posts.push(element);

        let titleElement = element.querySelector(".oajrlxb2.g5ia77u1.qu0x051f.esr5mh6w.e9989ue4.r7d6kgcz.rq0escxv.nhd2j8a9.nc684nl6.p7hjln8o.kvgmc6g5.cxmmr5t8.oygrvhab.hcukyx3x.jb3vyjys.rz4wbd8a.qt6c0cv9.a8nywdso.i1ao9s8h.esuyzwwr.f1sip0of.lzcic4wl.oo9gr5id.gpro0wi8")
        let descriptionElement = element.querySelector(".d2edcug0.hpfvmrgz.qv66sw1b.c1et5uql.rrkovp55.a8c37x1j.keod5gw0.nxhoafnm.aigsh9s9.d3f4x2em.fe6kdd0r.mau55g9w.c8b282yb.iv3no6db.jq4qci2q.a3bd9o3v.knj5qynh.oo9gr5id.hzawbc8m");

        let desc = !!descriptionElement ? descriptionElement.innerText : "No Description"
        let title = !!titleElement ? titleElement.href.split('?')[0] : "Group Name"
        chrome.runtime.sendMessage({ Title: title, Description: desc, Index: posts.indexOf(element) })
      })
    }
  };

  // Create an observer instance linked to the callback function
  const observer = new MutationObserver(callback);

  observer.observe(targetNode, { childList: true });
}

set()


MutationObserver = window.MutationObserver || window.WebKitMutationObserver;

var observerw = new MutationObserver(function (mutations, observer) {

  console.log("Sehfe Deyisdi")
  set()

});

// define what element should be observed by the observer
// and what types of mutations trigger the callback
observerw.observe(document.querySelector('title'), {
  subtree: true, characterData: true, childList: true
  //...
});



