const { defineConfig } = require('@vue/cli-service')
module.exports = defineConfig({
  transpileDependencies: true,
  productionSourceMap: true,  // map source để có thể debug
  lintOnSave: false, // tắt eslint chặn các rule 
   configureWebpack: {
    devtool: 'source-map',
    optimization: {
      minimize: false // tránh bị minify 
    }
  }
})
