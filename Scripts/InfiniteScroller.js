function InfiniteScroller(containerEl) {
    //console.log('InfiniteScrollGeneric invoked');
    let currentPage = 1;
    let total = -1;
    let alreadyFetching = false;
    let scrollCallDelaying = false;

    var exposed = {
        limit: 10,
        apiCallUrl: '',
        setApiCallData: function () {
            return { currentPage: 1 };
        },
        showFetchedData: function (data, removeExistingData) { },
        loadInitialData: loadInitialData
    };

    function loadInitialData() {
        currentPage = 1;
        total = -1;
        loadAdditionalData();
    }

    async function loadAdditionalData() {
        try {
            if (alreadyFetching) return;
            alreadyFetching = true;
            showLoadingSign(containerEl);
            let quotesBatch = await FetchNextPageData(currentPage);
            exposed.showFetchedData(quotesBatch, currentPage == 1);

        } catch (err) {
            console.log('Error occurred: ' + err.message);
        }
        alreadyFetching = false;
        hideLoadingSign(containerEl);
    }
    async function FetchNextPageData(page) {
        let params = exposed.setApiCallData();
        params.pageNumber = currentPage;
        params.limit = exposed.limit;
        let response = await postData(exposed.apiCallUrl, params)
        if (response.ok) {
            let result = await response.json();
            total = result.total;
            return result.data;
        }
        else
            throw new Error(`An error occurred ${response.status}`);
    };

    //Check if next page being loaded has data within total range
    function hasMoreData() {
        let startIndex = currentPage * exposed.limit + 1;
        return total == -1 || startIndex <= total;
    };

    containerEl.addEventListener('scroll', scrollCaller.bind(this), { passive: true });

    function scrollCaller() {
        let { scrollTop, scrollHeight, clientHeight } = containerEl;
        if (scrollTop + clientHeight + 2 > scrollHeight) {
            if (!scrollCallDelaying) {
                scrollCallDelaying = true;
                if (hasMoreData()) {
                    currentPage++;
                    loadAdditionalData();
                }
                setTimeout(function () { scrollCallDelaying = false }, 700);
            }
        }
    }

    //Ref: https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API/Using_Fetch
    async function postData(url = "", data = {}) {
        // Default options are marked with *
        const response = await fetch(url, {
            method: "POST", // *GET, POST, PUT, DELETE, etc.
            mode: "cors", // no-cors, *cors, same-origin
            cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
            credentials: "same-origin", // include, *same-origin, omit
            headers: {
                "Content-Type": "application/json",
                // 'Content-Type': 'application/x-www-form-urlencoded',
            },
            redirect: "follow", // manual, *follow, error
            referrerPolicy: "no-referrer", // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
            body: JSON.stringify(data), // body data type must match "Content-Type" header
        });
        return response;
    }

    return exposed;
}
