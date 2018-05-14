var app = new Vue({
    el: "#app",
    data: {
        apiurl: 'https://rohlikapi.azurewebsites.net/api/GetAllProducts',
        showLoader: true,
        searchString: "",
        products: [],
        time: null,
        regex: false
    },

    computed: {
        filteredProducts: function () {
            var productArray = this.products;
            var searchString = this.searchString;

            if (!searchString) {
                return productArray.slice(0, 100);
            }

            var parameters = { search: searchString }
            var parametersString = $.param(parameters);
            var newLocation = location.protocol + '//' + location.host + location.pathname + "?" + parametersString;
            history.pushState(newLocation, "", newLocation);

            searchString = searchString.trim().toLowerCase();

            if (this.regex) {
                return this.filterProductsRegex(productArray, searchString);
            }
            return this.filterProducts(productArray, searchString);
        }
    },

    created: function () {
        this.setUrlFromParam();
        this.fetchData();
    },

    methods: {
        formatDate: function (dateString) {
            var date = new Date(dateString);
            var month = parseInt(date.getMonth()) + 1;
            return date.getDate() + "." + month + "." + date.getFullYear() + " " + ("0" + date.getHours()).slice(-2) + ":" + ("0" + date.getMinutes()).slice(-2);
        },

        fetchData: function () {
            var self = this;
            $.get(self.apiurl, function (data) {
                self.products = data.Products;
                self.products.forEach(function(item) {
                    item.Size = (item.Price / item.PPU).toFixed(2);
                }, this);

                self.time = self.formatDate(data.SyncTime);
                self.showLoader = !self.showLoader;
            });
        },

        setUrlFromParam: function () {
            var searchParam = window.location.search.substring(1).split("=")[1];
            if (searchParam) {
                var decodedSearchParam = decodeURIComponent(searchParam);
                this.searchString = decodedSearchParam;
            };
        },

        filterProductsRegex: function (productArray, searchString) {
            var regex = new RegExp(searchString);
            return productArray.filter(function (item) {
                if (regex.test(item.Sname)) {
                    return item;
                }
            }).slice(0, 100);
        },

        filterProducts: function (productArray, searchString) {
            var searchItems = searchString.split(" ");
            var negativeItems = searchItems.filter(function (item) {
                return item.charAt(0) == '-' && item.length > 1;
            });

            negativeItems = negativeItems.map(function (negativeItem) {
                return negativeItem.substring(1);
            });

            var positiveItems = searchItems.filter(function (item) {
                return item.charAt(0) != '-';
            });


            function isValid(item) {
                var negativeFound = false;
                for (i = 0; i < negativeItems.length; i++) {
                    if (item.Sname.indexOf(negativeItems[i]) !== -1) {
                        negativeFound = true;
                        break;
                    };
                }
                if (negativeFound) {
                    return false;
                }
                var count = 0;
                positiveItems.forEach(function (searchWord) {
                    if (item.Sname.indexOf(searchWord) !== -1) {
                        count++;
                    }
                });
                if (count == positiveItems.length) {
                    return true;
                };
            }

            return productArray.filter(isValid, negativeItems, positiveItems).slice(0, 100);
        }
    }
}
)
