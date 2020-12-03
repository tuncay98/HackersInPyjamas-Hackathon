chrome.runtime.onMessage.addListener(
    function(request, sender, sendResponse) {
      if( request.message === "clicked_browser_action" ) {
        var firstHref = "google.com";
  
        console.log(firstHref);
  
        // This line is new!
        chrome.runtime.sendMessage({"message": "open_new_tab", "url": firstHref});
      }
    }
  );



  const cssFile = 
  `
  position: absolute;
  width: 100px;
  height: 20px;
  background: rgba(137,205,229,.5);
  z-index: 5;
  right: 0;
  `


  let appendElement = (element) => {

    let ctx =  document.createElement("div");
    let txt = document.createElement("span");
    
    element.appendChild(ctx)
    ctx.appendChild(txt)
    
    ctx.style = cssFile;
    
    txt.innerText = "Report"
    txt.style.color = "white"

  }


  const targetNode = document.querySelector('[role="feed"]');

  const callback = function(mutationsList, observer) {
    // Use traditional 'for loops' for IE 11
    for(const mutation of mutationsList) {
        mutation.addedNodes.forEach(element => {
            appendElement(element)
            let titleElement = element.querySelector(".gmql0nx0.l94mrbxd.p1ri9a11.lzcic4wl.aahdfvyu.hzawbc8m strong span")
            let descriptionElement = document.querySelector(".d2edcug0.hpfvmrgz.qv66sw1b.c1et5uql.rrkovp55.a8c37x1j.keod5gw0.nxhoafnm.aigsh9s9.d3f4x2em.fe6kdd0r.mau55g9w.c8b282yb.iv3no6db.jq4qci2q.a3bd9o3v.knj5qynh.oo9gr5id.hzawbc8m");

            let desc = !!descriptionElement?descriptionElement.innerText:"No Description"
            let title = !!titleElement?titleElement.innerText:"Group Name"
            chrome.runtime.sendMessage({title: title, description: desc})
        })

    }
};

// Create an observer instance linked to the callback function
const observer = new MutationObserver(callback);

observer.observe(targetNode, {childList: true});