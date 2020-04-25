class Assert {
    static Equal(a, b) {
        if (a !== b)
            throw `${a} !== ${b}`
    }
    static Single(array) {
        if (!array || array.length != 1)
            throw `${array}.length != 1`
    }
}

export default Assert

