$(document).ready(function(){

var svg = d3.select("#cloud");
///////////////////////////////////////// screen height and width ///////////////////////////////////////////////

//var w = 1000, h = 700;
//var w = screen.availWidth - 300, h = screen.availHeight;
function jsUpdateSize() {
    var width = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
    var height = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
    return [width, height];
};

window.onload = jsUpdateSize;
window.onresize = jsUpdateSize;

var reSize = jsUpdateSize();
var w = reSize[0] - 300, h = reSize[1];

// this thing need every refresh button clicked like it has no tomorrow!
///////////////////////////////////////// screen height and width ///////////////////////////////////////////////
var startCase = ($("#mapTitle").data("id"));
    previousCase = ""; 
    lastCase = previousCase,
    maxCircles = 24;

radialLocation = function (center, angle, radius) {
    var x, y;
    x = Math.round(center.x + radius * Math.cos(angle * Math.PI / 180));
    y = Math.round(center.y + radius * Math.sin(angle * Math.PI / 180));
    return {
        "x": x,
        "y": y
    };
};

pickColor = function (data) {
    if (data == "foll") {
        return "green";
    } else if (data == "refd") {
        return "#FF8C00";
    } else if (data == "dist") {
        return "red";
    }
};

removeAll = function () {
    d3.selectAll("circle").remove();
    d3.selectAll("rect").remove();
    d3.selectAll("line").remove();
    d3.selectAll("text").remove();
    d3.selectAll("marker").remove();
    d3.selectAll("defs").remove();
    d3.selectAll("#svg-tooltip").remove();
      
};

function Map(data) {
    var refCases = data.referredCases;

    refCases.sort(function (obj1, obj2) {
        return obj2.strength - obj1.strength;
    });

    // Creating variables for the SVG
    var padding = 30, radius = 225, increment = 30, radiusIncrement = 125, start = 180, xPoints = [], yPoints = [], xIndex = 0, yIndex = 0, showDetails, hideDetails;
    var largeRadius = 60, smallRadius = 25;
    var lineRadius = radius - smallRadius;
    var linkRadiusIndex1 = 0, linkRadiusIndex2 = 0;
    var center = {
        "x": w / 2,
        "y": h / 2
    };
    var tooltip = Tooltip("svg-tooltip", 230);

    //Retrieving all the data on the case ruling properties
    var rulingData = [], rulingDataRect = [], strengthData = [];

    for (var i = 0; i < refCases.length; i++) {
        rulingDataRect[i] = refCases[i].ruling;
    }

    packageData = function (dataToPackage) {
        var count = refCases.length,
            preppedData = [],
            circleLength = (360 / increment);

        if ((count * increment) <= 360) {
            preppedData[0] = dataToPackage;
        } else if (((count * increment) > 360) && ((count * increment) <= 720)) {
            preppedData[0] = dataToPackage.slice(0, circleLength);
            preppedData[1] = dataToPackage.slice(circleLength);
        } else if (((count * increment) > 720) && ((count * increment) <= 1080)) {
            preppedData[0] = dataToPackage.slice(0, circleLength);
            preppedData[1] = dataToPackage.slice(circleLength, (circleLength * 2));
            preppedData[2] = dataToPackage.slice((circleLength * 2));
        }
        return preppedData;
    }

    var dataForData = packageData(refCases);
    var firstCircleCount = dataForData[0].length;

    for (var i = 0; i < firstCircleCount; i++) {
        rulingData[i] = refCases[i].ruling;
    }

    //Retrieving all the data on the case strengths
    for (var j = 0; j < firstCircleCount; j++) {
        strengthData[j] = refCases[j].strength;
    }

    /**************************************Tooltip Functions*********************************/

    showDetails = function (d, i) {
        var content,
            color = d3.select(this).attr("stroke"),
            ruling;
        if (!d.ruling) {
            content = '<p class="main">' + data.case[0].mainCase.caseName + '</span></p>';
            content += '<hr class="tooltip-hr">';
            content += '<p class="main">  </span></p>';
        }
        else {
            if (d.ruling == "foll") {
                ruling = "Applied";
            } else if (d.ruling == "dist") {
                ruling = "Overruled";
            } else {
                ruling = "Referred";
            }
            content = '<p class="main"><a href=case_notes/showcase.aspx?id='+ d.caseID + ' target="_blank">' + d.caseName + '</a></span></p>';
            content += '<hr class="tooltip-hr">';
            content += '<p class="main">' + d.court + '</span></p>';
            content += '<hr class="tooltip-hr">';
            content += '<p class="main">' + ruling + '</span></p>';
             
        }
        tooltip.showTooltip(content, d3.event);
        if (d.ruling)
            return d3.select(this).style("opacity", "0.3").style("cursor", "pointer");//.attr("class", "circleHover");//.attr("fill", color).attr("stroke-width", d3.select(this).attr("stroke-width") * .6);
        else
            return d3.select(this).attr("class", "circleHover").attr("fill", color).attr("stroke-width", d3.select(this).attr("stroke-width") * .6);
    }

    hideDetails = function (d, i) {
        tooltip.hideTooltip();
        if (d.ruling)
            return d3.select(this).style("opacity", "1");//.attr("class", "referredCircles").classed("circleHover", false);
        else
            return d3.select(this).attr("class", "mainCase");
    }

    showLinkDetails = function (d, i) {
        var content;
        if (d == "dist") {
            content = '<p class="main"> Overruled </span></p>';
        } else if (d == "foll") {
            content = '<p class="main"> Applied </span></p>';
        } else {
            content = '<p class="main"> Referred </span></p>';
        }
        tooltip.showTooltip(content, d3.event);
    }

    hideLinkDetails = function () {
        tooltip.hideTooltip();
    }

    var defs = svg.append("svg:defs");

    var gradient = defs.append("svg:linearGradient")
    .attr("id", "mainCircleGradient")
    .attr("x1", "0%")
    .attr("y1", "0%")
    .attr("x2", "100%")
    .attr("y2", "100%")
    .attr("spreadMethod", "pad");

    gradient.append("svg:stop")
        .attr("offset", "0%")
        .attr("stop-color", "#6CA6CD")
        .attr("stop-opacity", 1);

    gradient.append("svg:stop")
        .attr("offset", "100%")
        .attr("stop-color", "#00688B")
        .attr("stop-opacity", 1);

    var rGradient = defs.append("svg:linearGradient")
        .attr("id", "rCircleGradient")
        .attr("x1", "0%")
        .attr("y1", "0%")
        .attr("x2", "100%")
        .attr("y2", "100%")
        .attr("spreadMethod", "pad");

    rGradient.append("svg:stop")
        .attr("offset", "0%")
        .attr("stop-color", "#87CEFF")
        .attr("stop-opacity", 1);

    rGradient.append("svg:stop")
        .attr("offset", "100%")
        .attr("stop-color", "#00688B")
        .attr("stop-opacity", 1);

    var silverGradient = defs.append("svg:linearGradient")
    .attr("id", "silverGradient")
    .attr("x1", "0%")
    .attr("y1", "0%")
    .attr("x2", "100%")
    .attr("y2", "100%")
    .attr("spreadMethod", "pad");

    silverGradient.append("svg:stop")
        .attr("offset", "0%")
        .attr("stop-color", "#FFFFFF")
        .attr("stop-opacity", 1);

    silverGradient.append("svg:stop")
        .attr("offset", "100%")
        .attr("stop-color", "#F5F5F5")
        .attr("stop-opacity", 1);

    /*************************************************END***************************************/

    //Creating the visual and setting the attributes & text for the main case of the map
    updateMainCircle = function () {
        var color = "#00688B",
            mainCircle = svg.selectAll("circle")
                            .data(data.case)
                            .enter()
                            .append("circle")
                            .on("mouseover", showDetails);//.on("mouseout", hideDetails);

        mainCircle.attr("class", "mainCase")
        .attr("stroke", color)
        .attr("cx", center.x)
        .attr("cy", center.y)
        .attr("r", 60);

        svg.selectAll("rect")
        .data(data.case)
        .enter()
        .append("rect")
        .attr("x", function (d, i) {
            return center.x - 75;
        })
        .attr("y", function (d, i) {
            return center.y - 13;
        })
        .attr("width", 150)
        .attr("height", 20)
        .attr("rx", 5)
        .attr("ry", 5)
        .attr("class", "mainlabelRect")
        .attr("stroke", color)
        .attr("fill", "url(#mainCircleGradient)");
    }

    //Add the text that shows in the middle of the main case circle
    updateMainLabel = function () {
        var caseTitle,
            mainCircle = svg.selectAll("circle")
        svg.selectAll("text")
        .data(data.case)
        .enter()
        .append("text")
        .text(function (d) {
            caseTitle = d.mainCase.caseName;
            if (caseTitle.length > 30) {
                caseTitle = caseTitle.substring(0, 30);
            }
            return caseTitle;
        })
        .attr("class", "mainCaseLabel")
        .attr("x", mainCircle.attr("cx") - mainCircle.attr("r") + padding - 40)
        .attr("y", mainCircle.attr("cy"));
    }

    //Creating the visual and setting the attributes and labels of the cases referred to by the main case
    updateReferredCircles = function () {
        createReferredCircles = function (data, radius, currentX, currentY, notFirstCircle) {
            //var referredCircles = svg.selectAll("circle")
            var group = svg.append("g"),
                referredCircles = group.selectAll("circle")
            .data(data)
            .enter()
            .append("circle")
            .on("mouseover", showDetails);//.on("mouseout", hideDetails);

            referredCircles.attr("cx", function (d, i) {
                var points;
                if (notFirstCircle === true && (currentX === 180 || currentX === 0 || currentX === 360)) {
                    points = radialLocation(center, currentX, radius + increment);
                }
                else {
                    points = radialLocation(center, currentX, radius);
                }
                if ((currentX + increment) > 360) {
                    currentX = (currentX + increment) - 360;
                } else {
                    currentX += increment;
                }
                xPoints[xIndex] = points.x;
                xIndex++;
                return points.x;
            })
            .attr("cy", function (d, i) {
                var points;
                if (notFirstCircle === true && (currentY === 270 || currentY === 90)) {
                    points = radialLocation(center, currentY, radius - increment);
                    if ((currentY + increment) > 360) {
                        currentY = (currentY + increment) - 360;
                    } else {
                        currentY += increment;
                    }
                }
                else {
                    points = radialLocation(center, currentY, radius);
                    if ((currentY + increment) > 360) {
                        currentY = (currentY + increment) - 360;
                    } else {
                        currentY += increment;
                    }
                }
                yPoints[yIndex] = points.y;
                yIndex++;
                return points.y;
            })
            .attr("r", function (d) {
                return smallRadius;// + (d.strength * 1.2);

            })
            .attr("class", "referredCircles")
            .attr("stroke", function (d) {
                return pickColor(d.ruling);
            })
            //.attr("stroke", "url(#rCircleGradient)")
            .attr("fill", "url(#rCircleGradient)");
        }

        var preppedData = packageData(refCases),
            circleNo = preppedData.length,
            dRadius = radius;

        for (var i = 0; i < circleNo; i++) {
            if (i === 0) {
                createReferredCircles(preppedData[i], dRadius, start, start);
            } else {
                createReferredCircles(preppedData[i], dRadius, start, start, true);
            }
            dRadius += radiusIncrement;
        }
    }
    //Creating the links that connect the referred case to the main case(Case in the center)
    updateLinks = function () {
        var currentAngleX = start,
            currentAngleY = start,
            currentAngleX2 = start,
            currentAngleY2 = start,
            count = 0,
            count2 = 0,
            arrows ;
        if($('#orientation').html().trim() == 'ref'){
            arrows = svg.append("svg:defs").selectAll("marker")
                .data(rulingData)
                .enter().append("svg:marker")
                .attr("viewBox", "0 -5 10 10")
                .attr("refX", 50)
                .attr("refY", 0)
                .attr("markerWidth", 6)
                .attr("markerHeight", 6)
                .attr("id", function (d) {
                    var val = count;
                    count++;
                    return val;
                })
                .attr("orient", "auto")
                .attr("fill", function (d) {
                    return pickColor(d);
                })
                .attr("stroke", function (d) {
                    return pickColor(d);
                })
                .append("svg:path")
                .attr("d", "M 0 0 L 10 -5 L 10 5");
        }else{
            arrows = svg.append("svg:defs").selectAll("marker")
                .data(rulingData)
                .enter().append("svg:marker")
                .attr("viewBox", "0 -5 10 10")
                .attr("refX", 50)
                .attr("refY", 0)
                .attr("markerWidth", 6)
                .attr("markerHeight", 6)
                .attr("id", function (d) {
                    var val = count;
                    count++;
                    return val;
                })
                .attr("orient", "auto")
                .attr("fill", function (d) {
                    return pickColor(d);
                })
                .attr("stroke", function (d) {
                    return pickColor(d);
                })
                .append("svg:path")
                .attr("d", "M0,-5L10,0L0,5");
        }

        var links = svg.selectAll("line")
        .data(rulingData)
        .enter()
        .append("line")
        .attr("class", "link");

        var scale = d3.scale.linear()
                            .domain([1, 50])
                            .range([1.5, 8]);

        links.attr("x1", function (d) {
            //The code below calculates the x position of the circle in the center with reference to the referred circle
            if (currentAngleX == 180 || currentAngleX == 360 || currentAngleX == 0) {
                var points = radialLocation(center, currentAngleX, largeRadius + 16);
            } else {
                var points = radialLocation(center, currentAngleX, largeRadius + 3);
            }
            if ((currentAngleX + increment) > 360) {
                currentAngleX = (currentAngleX + increment) - 360;
            } else {
                currentAngleX += increment;
            }
            return points.x;
        })
        .attr("y1", function (d) {
            //The code below calculates the y position of the circle in the center with reference to the referred circle
            var points = radialLocation(center, currentAngleY, largeRadius);
            if ((currentAngleY + increment) > 360) {
                currentAngleY = (currentAngleY + increment) - 360;
            } else {
                currentAngleY += increment;
            }
            return points.y;

        })
        .attr("x2", function (d) {
            //The code below calculates the x position of the referred circle with reference to the circle in the center
            //While taking into account the radius of the referred circle.
            var points = radialLocation(center, currentAngleX2, lineRadius); //- (refCases[linkRadiusIndex1].strength * 1.2));
            linkRadiusIndex1++;
            if ((currentAngleX2 + increment) > 360) {
                currentAngleX2 = (currentAngleX2 + increment) - 360;
            } else {
                currentAngleX2 += increment;
            }
            return points.x;
        })
        .attr("y2", function (d) {
            //The code below calculates the x position of the referred circle (Same as above x2)
            var points = radialLocation(center, currentAngleY2, lineRadius); //- (refCases[linkRadiusIndex2].strength * 1.2));
            linkRadiusIndex2++;
            if ((currentAngleY2 + increment) > 360) {
                currentAngleY2 = (currentAngleY2 + increment) - 360;
            } else {
                currentAngleY2 += increment;
            }
            return points.y;
        });

        links.attr("stroke", function (d) {
            return pickColor(d);
        })
        .attr("stroke-width", function (d, i) {
            return scale(strengthData[i]);
        });

        links.attr("marker-end", function (d) {
            var val = "url(#" + count2 + ")";
            count2++;
            return val;
        });

        links.on("mouseover", showLinkDetails)
        .on("mouseout", hideLinkDetails);
    }

    //Updata Link Gauges
    //updateLinkGauges = function () {
    //    var currentAngleX = start,
    //        currentAngleY = start,
    //        currentAngleX2 = start,
    //        currentAngleY2 = start;

    //    var scale = d3.scale.linear()
    //                        .domain([1, 50])
    //                        .range([10, 140]);

    //    var group = svg.append("g"),
    //        linkGauges = group.selectAll("line")
    //                            .data(strengthData)
    //                            .enter()
    //                            .append("line")
    //                            .attr("class", "linkGauge");

    //    linkGauges.attr("x1", function (d) {
    //        //The code below calculates the x position of the circle in the center with reference to the referred circle
    //        if (currentAngleX == 180 || currentAngleX == 360 || currentAngleX == 0) {
    //            var points = radialLocation(center, currentAngleX, largeRadius + 16);
    //        } else {
    //            var points = radialLocation(center, currentAngleX, largeRadius + 2);
    //        }
    //        if ((currentAngleX + increment) > 360) {
    //            currentAngleX = (currentAngleX + increment) - 360;
    //        } else {
    //            currentAngleX += increment;
    //        }
    //        return points.x;
    //    })
    //    .attr("y1", function (d) {
    //        //The code below calculates the y position of the circle in the center with reference to the referred circle
    //        var points = radialLocation(center, currentAngleY, largeRadius + 3);
    //        if ((currentAngleY + increment) > 360) {
    //            currentAngleY = (currentAngleY + increment) - 360;
    //        } else {
    //            currentAngleY += increment;
    //        }
    //        return points.y;
    //    })
    //    .attr("x2", function (d) {
    //        //The code below calculates the x position of the referred circle with reference to the circle in the center
    //        //While taking into account the radius of the referred circle.
    //        var radius = largeRadius + scale(d);
    //        var points = radialLocation(center, currentAngleX2, radius); // smallRadius + (d * 10));
    //        if ((currentAngleX2 + increment) > 360) {
    //            currentAngleX2 = (currentAngleX2 + increment) - 360;
    //        } else {
    //            currentAngleX2 += increment;
    //        }
    //        return points.x;
    //    })
    //    .attr("y2", function (d) {
    //        //The code below calculates the x position of the referred circle (Same as above x2)
    //        var radius = largeRadius + scale(d);
    //        var points = radialLocation(center, currentAngleY2, radius); // smallRadius + (d * 10));
    //        if ((currentAngleY2 + increment) > 360) {
    //            currentAngleY2 = (currentAngleY2 + increment) - 360;
    //        } else {
    //            currentAngleY2 += increment;
    //        }
    //        return points.y;
    //    });

    //    linkGauges
    //    .data(rulingData)
    //    .attr("stroke", function (d) {
    //        return pickColor(d);
    //    })
    //    .on("mouseover", showLinkDetails)
    //    .on("mouseout", hideLinkDetails);
    //}

    //Creating the rectangular containers for the labels
    updateLabelRects = function () {
        var xIndex = 0, yIndex = 0;

        createLabelRects = function (data, radius) {
            //var rects = svg.selectAll("rect")
            var group = svg.append("g");
            var linkGauges = group.selectAll("rect")
            .data(data) //.data(rulingDataRect)
            .enter()
            .append("rect")
            .on("mouseover", showDetails) //.on("mouseout", hideDetails)
            .attr("x", function (d, i) {
                var xPoint = xPoints[xIndex] - 70;
                xIndex++;
                return xPoint;
            })
            .attr("y", function (d, i) {
                var yPoint = yPoints[yIndex] - 10;
                yIndex++;
                return yPoint;
            })
            .attr("width", 140)
            .attr("height", 15)
            .attr("rx", 5)
            .attr("ry", 5)
            .attr("class", "labelRect")
            //.attr("stroke", function (d) {
            //    return pickColor(d);
            //})
            //.attr("fill", function (d) {
            //    return pickColor(d);
            //});
            .attr("fill", "url(#silverGradient)")
            .attr("stroke", "#D3D3D3");
        }

        //var preppedData = packageData(rulingDataRect);
        //var circleNo = preppedData.length;
        //var dRadius = radius;

        //for (var i = 0; i < circleNo; i++) {
        //    createLabelRects(preppedData[i], dRadius);
        //    dRadius += radiusIncrement;
        //}

        var preppedData = packageData(refCases),
            circleNo = preppedData.length,
            dRadius = radius;

        for (var i = 0; i < circleNo; i++) {
            createLabelRects(preppedData[i], dRadius);
            dRadius += radiusIncrement;
        }
    }

    //Add the text that shows in the middle of the Referred cases circles	
    updateReferredLabels = function () {
        var caseTitle,
            xIndex = 0,
            yIndex = 0;

        createLabels = function (data, radius) {
            //labels = svg.selectAll("text")
            var group = svg.append("g");
            var linkGauges = group.selectAll("text")
            .data(data)
            .enter()
            .append("text")
            .on("mouseover", showDetails)//.on("mouseout", hideDetails)
            .text(function (d) {
                caseTitle = d.caseName;
                if (typeof caseTitle !== "undefined") {
                    if (caseTitle.length > 30) {
                        caseTitle = caseTitle.substring(0, 29);
                    }
                }
                return caseTitle;
            })
            .attr("class", "rCaseLabel")
            .attr("x", function (d, i) {
                var xPoint = xPoints[xIndex] - 68;
                xIndex++;
                return xPoint;
            })
            .attr("y", function (d, i) {
                var yPoint = yPoints[yIndex];
                yIndex++;
                return yPoint;
            });
        }

        var preppedData = packageData(refCases),
            circleNo = preppedData.length,
            dRadius = radius;

        for (var i = 0; i < circleNo; i++) {
            createLabels(preppedData[i], dRadius);
            dRadius += radiusIncrement;
        }
    }

    update = function () {
        updateMainCircle();
        updateMainLabel();
        updateReferredCircles();
        updateLinks();
        //updateLinkGauges();
        updateLabelRects();
        updateReferredLabels();
    }

    $(update);

    $(function() {
        d3.selectAll(".referredCircles").on("click", function (d) {
            removeAll();
            lastCase = data.case[0].mainCase.caseID;
            return init(d.caseID);
        });
    });
}

function init(caseID) {
    var slideTracker = 1,
        mainCaseName,
        slides = [],
        $start = $("#start");

    function toTitleCase(str) {
        return str.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
    }

    function togglePrevButton() {
        var prev = $("#prev");
        if (caseID !== "" && caseID !== undefined) {
            prev.removeClass();
        } else {
            prev.attr('class', 'hidden');
        }
    }

    function toggleNextButton(slideCount) {
       var next = $("#next");
         var nextClass = $("#nextClassic");
        if (slideCount > 2) {
            next.removeClass();
            next.addClass("btnSave");
            nextClass.removeClass();
            nextClass.addClass("btnSave");
        } else {
            next.attr('class', 'hidden');
              nextClass.attr('class', 'hidden');
        }
    }

    function createMap(slides, mainCaseName) {
        var data = {
            "case": [
            {
                "mainCase": {
                    "caseName": mainCaseName,
                    "caseID": caseID
                }
            }],
            "referredCases": []
        };
       
        var v,
            u = 1,
            slideLength = slides.length;
           
        if (slideTracker > 1) {
            v = (slideTracker - 1) * maxCircles;
        } else {
            v = 1;
        }
        var checkloopno=0;
        while (u <= slides["" + slideTracker + ""]) {
          checkloopno++;
          if ( checkloopno>24)
            break;
            $('#datatbl').find("#" + v).each(function () {
          
           
                var caseID = $(this).find(".fileLinkTo").text(),
                    caseName = $(this).find(".refTitle").html(),
                    court = $(this).find(".court").text(),
                    ruling = $(this).find(".type").text(),
                    judgDate =$(this).find(".date").text(),
                     judgcite =$(this).find(".cite").text(),
                    referredCase = {};
                referredCase.caseID = caseID;
                referredCase.caseName = caseName;
                referredCase.court = toTitleCase(court);
                referredCase.ruling = ruling.toLowerCase();
                referredCase.strength = 10;
                data.referredCases.push(referredCase);
                
                        if (caseName.length  > 60){
                              caseName=caseName.substring(0,60) + "....";
                             }
          
                    var caseLinks1 = " <div class='boxRecentResult1'><h2 class='title2Case2'><a href='case_notes/showcase.aspx?id="+ caseID +"' target='_blank'  id='" + caseID + "'  data-title=''>" + caseName + "</a></h2>"
                    var caseLinks= " <p class='title3Case'><a href='#'>" + court + "</a><a style='color:#f47200; font-weight:bold;' href='#'>&nbsp;&nbsp;   " + ruling + "</a>]</p></div>"
                    $(".CasesCitation").append(caseLinks1 + caseLinks);
                     var classic ="<div class='boxResult'><div class='resultTitleCase' style=' width:95%;'><p class='tTitleCase' ><a href='case_notes/showcase.aspx?id="+ caseID +"' target='_blank'  id='" + caseID + "'  data-title=''>" + caseName + " </a></p><p class='title2Case'> <a href='#' style='color:#f47200;'>" + court + "</a>&nbsp;&nbsp; <span>" + judgDate +  "</span></p><p class='tItalic'><a href='#'></a><span class='tfontCase'>" + judgcite + "</span></p></div><div class='clear'></div></div>"
                     $("#ListCases").append(classic);


            });
            v++;
            u++;
        }
         //if ( checkloopno>22)
            //break;
       // alert(checkloopno);
        if (slideTracker === slideLength - 1) {
            slideTracker = 1;
        } else {
            slideTracker++;
        }
        toggleNextButton(slideLength);
        togglePrevButton();
        if ($start.hasClass("hidden")) {
            $start.removeClass();
        }
        return Map(data);
    }

    $.getData = function () { /////////////////////////////////////////////////////////////////////// to be removed 
        return $.ajax({
            url: 'precMapLoader.aspx?id=' + caseID +"&t="+$('#orientation').html().trim() + "&sIn="+ $("#P1").html() + "&classic="+ $("#P3").html(),
            dataType: 'html'
        }).promise();
    };

    var parseData = $.getData(); ///////////////////////////////////////////////////////////////////// to be modified

    parseData.done(function (results) {
   
     $(".CasesCitation").html('');
        if (caseID) {
            previousCase = caseID;
        }
        if ($start.hasClass("error")) {
            $start.removeClass();
            $start.insertBefore($("#next"));
        }
        $("#error").attr("class", "hidden").empty();
        $("#dbData").attr("class", "hidden").empty();
        $("#dbData").append(results);
        mainCaseName = $("#datatbl").data("title");
        //mainCaseName = toTitleCase(mainCaseName);
        $("#mapTitle").text(mainCaseName);
        var refine= $("#refine").text();
        var lefttitle = $("#LeftTitle").text();
        $("#RefinS").text(refine);
                var classicview =$("#classicview").text();
               if (classicview.indexOf("classic") >= 0){
                    $("#contRight").hide();
                    $("#colleft").hide();
                    $('#classic').show();       
                }

         $("#Referred").text(lefttitle);
        if (mainCaseName.indexOf("Related Result") > 0){ return false;}
        var tableRows = $('#datatbl tr'),
            slideCount = Math.ceil(tableRows.length / maxCircles),
            tracker = 0;
            for (var z = 1; z <= slideCount; z++) {
            sliceLength = z * maxCircles;
            if (tableRows.slice(tracker, maxCircles).length === maxCircles) {
                slides[z] = sliceLength - tracker;
                tracker = sliceLength;
            } else {
                slides[z] = tableRows.slice(tracker).length;
            }
        }
        createMap(slides, mainCaseName);
       
    });

    parseData.fail(function (results) {
        $("#mapTitle").text("Precedent Map");
        $("#prev").attr('class', 'hidden');
        if (previousCase !== "" && previousCase !== null) {
         $("#mapTitle").text("Precedent Map");
            $("#error").removeClass().text("Oops! This case is yet to have a precedent map.").append("<br/><button id='back'>Previous</button>");
            $("#start").attr("class", "error").appendTo("#error");
        } else {
             $("#mapTitle").text("Precedent Map");
            $("#error").removeClass().text("Oops! This case is yet to have a precedent map.").append("<br/><button>Go Back</button>");
        }
    });

    $("#error").on("click", "#back", function () {
        init(previousCase);
        ("#svg-tooltip").last().remove();
    });

    $("#start").on("click", "button", function () {
     $(".CasesCitation").html('');
        removeAll();
        init(startCase);
        ("#svg-tooltip").last().remove();
    });

    $("#next").click(function () {
     $(".CasesCitation").html('');
        removeAll();
        createMap(slides, mainCaseName);
    });

    $("#nextClassic").click(function () {
     $("#ListCases").html('');
        removeAll();
        createMap(slides, mainCaseName);
    });


    $("#prev").click(function () {
     $(".CasesCitation").html('');
        $("#P1").html('');
        removeAll();
        if (lastCase === undefined || lastCase === "") {
            lastCase = caseID;
        }
        init(lastCase);
        ("#svg-tooltip").last().remove();
    });
}

init(startCase);
});

