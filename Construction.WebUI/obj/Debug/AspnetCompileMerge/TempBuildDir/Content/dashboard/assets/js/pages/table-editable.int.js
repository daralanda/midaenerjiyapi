!function (o) {
    o(".table-editable").dataTable({
        columns: [{
            name: "id"
        }, {
            name: "age"
        }, {
            name: "qty"
        }, {
            name: "cost"
        }],
        bPaginate: !1,
        fnRowCallback: function (a, e, n, l) {
            var i = function (a, e) {
                var n = new o.fn.dataTable.Api(".table").cell("td.focus")
                    , l = n.data()
                    , t = document.createElement("div");
                t.innerHTML = l,
                    t.childNodes.innerHTML = e,
                    console.log("jml a new " + t.innerHTML),
                    n.data(t.innerHTML),
                    c(o(n.node())),
                    o("td.focus a").editable({
                        mode: "inline",
                        success: i
                    })
            };
            o(".editable").editable({
                mode: "inline",
                success: i
            })
        },
        autoFill: {
            columns: [1, 2]
        },
        keys: !0
    });
    function c(a) {
        var e = a.attr("data-original-value");
        if (e) {
            var n = a.text();
            isNaN(e) || (e = parseFloat(e)),
                isNaN(n) || (n = parseFloat(n)),
                e === n ? a.removeClass("cat-cell-modified").addClass("cat-cell-original") : a.removeClass("cat-cell-original").addClass("cat-cell-modified")
        }
    }
    o("a[data-pk]").on("hidden", function (a, e) {
        c(o(this).parent("td"))
    }),
        o(".table").DataTable().on("autoFill", function (a, e, n) {
            o.each(n, function (a, e) {
                var n = e[0].cell;
                c(o(n.node()))
            })
        })
}(jQuery);
