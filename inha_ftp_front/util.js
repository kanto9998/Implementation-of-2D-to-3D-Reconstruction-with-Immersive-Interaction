

function onUnity_Play() {
	//filename_json();

	window.open( "./unity/index.html" );
}

function filename_json() {
}


function onProfile_Image_File_Play() {
	var		Image_File_Node = document.getElementById( "profile_img_file" );

	Image_File_Node.click();
}

function onProfile_Image_File(e) {

	//console.log( document.querySelector('input[type=file]').files );

	var			file = document.querySelector('input[type=file]').files[ 0 ];
	//console.log( document.getElementById( "profile_img_file" ) );
	//console.log( file );

    var 	reader = new FileReader();
	//reader.readAsBinaryString( file ); 				//파일을 읽는 메서드 
    reader.readAsDataURL( file ); 				//파일을 읽는 메서드 
	
	reader.onload = function() {
		//var		icon_img = document.getElementById( "profile_img" );

		console.log( reader.result );
		console.log( file.name );

		//icon_img.src = reader.result;

		//프로필 이미지
		//sessionStorage.setItem( "ProfileImage", reader.result );
		//sessionStorage.setItem( "ProfileImage", reader.result );
	}
}