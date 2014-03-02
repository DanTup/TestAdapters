module.exports = function (onComplete) {
	var onComplete = onComplete;
	var specs = [];

	this.jasmineDone = function () {
		if (onComplete)
			onComplete();
		console.log("<Tests>");
		for (var i = 0; i < specs.length; i++) {
			specDetails(specs[i]);
		}
		console.log("</Tests>");
	};

	this.specDone = function (result) {
		specs.push(result);
	};

	return this;

	function specDetails(result) {
		//console.log(result);

		console.log("	<Test>")
		console.log("		<Name>%s</Name>", xmlEncode(result.id))
		console.log("		<DisplayName>%s</DisplayName>", xmlEncode(result.fullName))
		//console.log("		<CodeFilePath>DannyFake.js</CodeFilePath>")
		//console.log("		<LineNumber>1</LineNumber>")
		if (result.status !== 'disabled') {
			console.log("		<Outcome>%s</Outcome>", result.status === 'passed' ? 'Passed' : result.status === 'failed' ? 'Failed' : 'Skipped')
			if (result.status === 'failed') {
				console.log("		<ErrorMessage>%s</ErrorMessage>", xmlEncode(result.failedExpectations[0].message))
				console.log("		<ErrorStackTrace>%s</ErrorStackTrace>", xmlEncode(result.failedExpectations[0].stack))
			}
		}
		console.log("	</Test>")
	}

	function xmlEncode(s) {
		if (s == null)
			return null;

		return s
			.replace(/&/g, '&amp;')
			.replace(/</g, '&lt;')
			.replace(/>/g, '&gt;')
			.replace(/"/g, '&quot;')
			.replace(/'/g, '&apos;');
	}
};

