interface Array<T> {
    sortBy(callback: (x: T) => any): Array<T>;
    sortByDesc(callback: (x: T) => any): Array<T>;
    sortByIdList(idList: string[], balanceList: number[]): Array<T>;
    groupBy<T>(xs: T[], key: string) : {[key: string]: T[]}

}

// THANKS: https://gist.github.com/robmathers/1830ce09695f759bf2c4df15c29dd22d
Array.prototype.groupBy = function(data, key) { // `data` is an array of objects, `key` is the key (or property accessor) to group by
  // reduce runs this anonymous function on each element of `data` (the `item` parameter,
  // returning the `storage` parameter at the end
  return data.reduce(function(storage, item) {
    // get the first instance of the key by which we're grouping
    var group = item[key];
    
    // set `storage` for this instance of group to the outer scope (if not empty) or initialize it
    storage[group] = storage[group] || [];
    
    // add this item to its group within `storage`
    storage[group].push(item);
    
    // return the updated storage to the reduce function, which will then loop through the next 
    return storage; 
  }, {}); // {} is the initial value of the storage
};

Array.prototype.sortBy = function (callback: (x) => any): [] {
    let newArray = this.slice();
    newArray.sort((left, right) => {
        let leftVal = callback(left);
        let rightVal = callback(right);
        if (leftVal < rightVal) {
            return -1;
        } else if (leftVal > rightVal) {
            return 1;
        } else {
            return 0;
        }
    });
    return newArray;
};

Array.prototype.sortByDesc = function (callback: (x) => any): [] {
    let newArray = this.slice();
    newArray.sort((left, right) => {
        let leftVal = callback(left);
        let rightVal = callback(right);
        if (leftVal < rightVal) {
            return 1;
        } else if (leftVal > rightVal) {
            return -1;
        } else {
            return 0;
        }
    });
    return newArray;
};

Array.prototype.sortByIdList = function (idList: string[], balanceList: number[]): any[] {
    let newArray = [];
    idList = idList || [];
    balanceList = balanceList || [];
    if (balanceList && balanceList.length != idList.length) {
        console.warn("idList and balanceList must have the same length! Returning list unsorted.");
        return this;
    }
    for (let i = 0; i < idList.length; i++) {
        let obj = this.filter(obj => obj.id == idList[i])[0];
        if (this.length && balanceList) {
            obj.__balance = balanceList[i];
            obj.__negative = balanceList[i] < 0;
        }
        newArray.push(obj);
    }

    return newArray;
}


