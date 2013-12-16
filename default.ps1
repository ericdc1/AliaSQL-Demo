properties {
	$projectName = "Demo"
    $version = null-coalesce $version ("1.0." + (get-date -format "yyyy"))

    $fullversion = $version + "." + (get-date -format "MMdd")  
    #http://stackoverflow.com/questions/10936828/how-do-i-control-which-version-of-an-msbuild-file-is-used-between-net4-and-4-5r
    $visualstudioversion = null-coalesce $visualstudioversion "12.0"
	$projectConfig = "Release"
	$base_dir = resolve-path .
	$source_dir = "$base_dir\source"
	$webapp_dir = "$source_dir\$projectName.Website" 
	$unitTestAssembly = "$projectName.UnitTests.dll"
	$xunitPath = "$source_dir\packages\xunit.runners.1.9.2\tools\xunit.console.clr4.exe"
	$AliaSQLPath = "$base_dir\lib\AliaSQL\AliaSQL.exe"
    $unitTest_dir = "$base_dir\source\$projectName.UnitTests"
	$zipPath = "$base_dir\lib\7zip\7za.exe"
	$build_dir = null-coalesce $build_dir "$base_dir\build"
	$test_dir = "$build_dir\test"
	$testCopyIgnorePath = "_ReSharper"
	$package_dir = "$build_dir\Latest"	  
	$package_file = "$build_dir\" + $projectName + "-" + $version +"_package.zip"
	$databaseServer = null-coalesce $databaseServer "localhost\sqlexpress"
	$databaseScripts = "$source_dir\Database.Demo"
	$databaseName = $projectName
    $SqlPackage = "C:\Program Files (x86)\Microsoft SQL Server\110\DAC\bin\SqlPackage.exe"
}

task default -depends Init, Compile, UpdateDatabase, Test, PopulateTestData
task ci -depends Init, Compile, UpdateDatabase, Test, Package


task Init {
    #delete the build directories, but leave any artifacts at the root then put it back
    delete_directory "$build_dir"
    create_directory "$build_dir"
}

task Compile -depends Init {
    write-host "Restore solution-level nuget packages"
    exec { & $source_dir\.nuget\nuget.exe restore $source_dir\$projectName.sln }
	
    Update-AssemblyInfoFiles $fullversion
    msbuild /t:clean /v:q /nologo /p:Configuration=$projectConfig $source_dir\$projectName.sln /p:VisualStudioVersion=$visualstudioversion 
    msbuild /t:build /v:q /nologo /p:Configuration=$projectConfig $source_dir\$projectName.sln /p:VisualStudioVersion=$visualstudioversion 
    copy_website_files "$webapp_dir" "$package_dir\web"

    #put the assemblyinfo version number back to .0.0 so we don't have to check in the changes on every local build
    Update-AssemblyInfoFiles ($version.Replace($version.Split('.')[-1], "0") + ".0") #cleans the version's year to something like 3.0.0.0
}

task Test {
    write-host "Testing $unitTestAssembly"
    copy_all_assemblies_for_test "$test_dir"
	exec { & $xunitPath $test_dir\$unitTestAssembly /xml $build_dir\UnitTestResult.xml}      
}

task GenerateDatabaseDiff {
    create-dbdiffscript $databaseName $databaseScripts
}

task RebuildDatabase {
    exec { & $AliaSQLPath Rebuild $databaseServer "$databaseName" "$databaseScripts\Scripts"}
}

task UpdateDatabase {
    exec { & $AliaSQLPath Update $databaseServer "$databaseName" "$databaseScripts\Scripts"}
}

task PopulateTestData { 
    exec { &  $AliaSQLPath TestData $databaseServer " $databaseName" "$databaseScripts\Scripts"}
}

task Baseline { 
    exec { &  $AliaSQLPath Baseline $databaseServer " $databaseName" "$databaseScripts\Scripts"}
}

task Package -depends Compile {

    write-host "Clean package directory"
    delete_directory $package_dir
   
    write-host "Copy web app"
    copy_website_files "$webapp_dir" "$package_dir\web" 

    write-host "Copy in database scripts"
    copy_files "$databaseScripts\scripts" "$package_dir\database\"

    write-host "Copy AliaSQL tool so scripts can be ran"
    Copy-Item "$AliaSQLPath"  "$package_dir\database\AliaSQL.exe"

    write-host "Create batch files to run db updates"
    create-dbdeployscript "Update" "$package_dir\database\_Update-Database.bat"
    create-dbdeployscript "Rebuild" "$package_dir\database\_Rebuild-Database.bat"
    create-dbdeployscript "Baseline"  "$package_dir\database\_Baseline-Database.bat"
    create-dbdeployscript "TestData" "$package_dir\database\_TestData-Database.bat"

    write-host "Zip it up"
	zip_directory $package_dir $package_file 
}
 
function global:zip_directory($directory,$file) {
    write-host "Zipping folder: " $test_assembly
    delete_file $file
    cd $directory
    & "$zipPath" a -mx=9 -r $file
    cd $base_dir
}

function global:copy_website_files($source,$destination){
    $exclude = @('*.user','*.dtd','*.tt','*.cs','*.csproj','*.vb','*.vbproj','*.orig', '*.log') 
    copy_files $source $destination $exclude
	delete_directory "$destination\obj"
     while (Get-ChildItem $destination -recurse | where {!@(Get-ChildItem -force $_.fullname)} | Test-Path) {
        Get-ChildItem $destination -recurse | where {!@(Get-ChildItem -force $_.fullname)} | Remove-Item
    }
}

function global:copy_files($source,$destination,$exclude=@()){    
    create_directory $destination
    Get-ChildItem $source -Recurse -Exclude $exclude -ErrorAction SilentlyContinue | Copy-Item -ErrorAction SilentlyContinue -Destination {Join-Path $destination $_.FullName.Substring($source.length)} 
}

function global:Copy_and_flatten ($source,$filter,$dest) {
  ls "$source" -filter "$filter"  -r | Where-Object{!$_.FullName.Contains("$testCopyIgnorePath") -and !$_.FullName.Contains("packages") }| cp -dest "$dest" -force
}

function global:copy_all_assemblies_for_test($destination){
  create_directory "$destination"
  Copy_and_flatten "$source_dir" *.exe "$destination"
  Copy_and_flatten "$source_dir" *.dll "$destination"
  Copy_and_flatten "$source_dir" *.config "$destination"
  Copy_and_flatten "$source_dir" *.xml "$destination"
  Copy_and_flatten "$source_dir" *.pdb "$destination"
}

function global:delete_file($file) {
    if($file) { remove-item $file -force -ErrorAction SilentlyContinue | out-null } 
}

function global:delete_directory($directory_name)
{
  rd $directory_name -recurse -force  -ErrorAction SilentlyContinue | out-null
}

function global:delete_files_in_dir($dir)
{
	get-childitem $dir -recurse | foreach ($_) {remove-item $_.fullname}
}

function global:create_directory($directory_name)
{
  mkdir $directory_name  -ErrorAction SilentlyContinue  | out-null
}

function create-dbdeployscript($verb, $filename)
{ 
"@echo off
set /p serverName=""DB Server: "" %=%
AliaSQL.exe $verb %serverName% $databaseName . 
pause" | out-file $filename -encoding "ASCII"       
}


function Update-AssemblyInfoFiles ([string] $version, [System.Array] $excludes = $null) {
 
#-------------------------------------------------------------------------------
# Update version numbers of AssemblyInfo.cs
# adapted from: http://www.luisrocha.net/2009/11/setting-assembly-version-with-windows.html
#-------------------------------------------------------------------------------
 
	if ($version -notmatch "[0-9]+(\.([0-9]+|\*)){1,3}") {
		Write-Error "Version number incorrect format: $version"
	}
	
	$versionPattern = 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
	$versionAssembly = 'AssemblyVersion("' + $version + '")';
	$versionFilePattern = 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
	$versionAssemblyFile = 'AssemblyFileVersion("' + $version + '")';
 
	Get-ChildItem -r -filter AssemblyInfo.* | % {
		$filename = $_.fullname
		
		$update_assembly_and_file = $true
		
		# set an exclude flag where only AssemblyFileVersion is set
		if ($excludes -ne $null)
			{ $excludes | % { if ($filename -match $_) { $update_assembly_and_file = $false	} } }
		
		# see http://stackoverflow.com/questions/3057673/powershell-locking-file
		# I am getting really funky locking issues. 
		# The code block below should be:
		#     (get-content $filename) | % {$_ -replace $versionPattern, $version } | set-content $filename
 
		$tmp = ($file + ".tmp")
		if (test-path ($tmp)) { remove-item $tmp }
 
		if ($update_assembly_and_file) {
			(get-content $filename) | % {$_ -replace $versionFilePattern, $versionAssemblyFile } | % {$_ -replace $versionPattern, $versionAssembly }  > $tmp
			write-host Updating file AssemblyInfo and AssemblyFileInfo: $filename --> $versionAssembly / $versionAssemblyFile
		} else {
			(get-content $filename) | % {$_ -replace $versionFilePattern, $versionAssemblyFile } > $tmp
			write-host Updating file AssemblyInfo only: $filename --> $versionAssemblyFile
		}
 
		if (test-path ($filename)) { remove-item $filename }
        #diff tools aren't too happy with Unicode files - change it to ansi
        Set-Content $tmp -Encoding ASCII -Value (Get-Content $tmp)
		move-item $tmp $filename -force		
 
	}
}

function create-dbdiffscript([string]$datebase_name, [string]$database_scripts)
{
    $databaseName_Original = "$datebase_name" + "_Original"
    $databaseScriptsUpdate = "$database_scripts\Scripts\Update"
    $newScriptName = ((Get-ChildItem $databaseScriptsUpdate -filter "*.sql" | ForEach-Object {[int]$_.Name.Substring(0, 4)} | Sort-Object)[-1] + 1).ToString("0000-") + "$datebase_name" + ".sql.temp"

    write-host "Building original database..."
    exec { & $AliaSQLPath Rebuild $databaseServer "$databaseName_Original" "$database_scripts\Scripts"}
    
    write-host "`n`nGenerating the diff script"
    #generate the needed .dacpac (we'll delete it later)
    exec { & $SqlPackage /a:Extract /ssn:.\SQLEXPRESS /sdn:"$datebase_name" /tf:$databaseScripts\$datebase_name.dacpac }
    #generate the diff script
    exec { & $SqlPackage /a:Script /op:$databaseScriptsUpdate\$newScriptName /sf:$databaseScripts\$datebase_name.dacpac /tsn:.\SQLEXPRESS /tdn:"$databaseName_Original"}
    
    write-host "`n`nCleaning up generated script..."
    $scriptLines = Get-Content $databaseScriptsUpdate\$newScriptName
    Clear-Content $databaseScriptsUpdate\$newScriptName

    $passedLastSqlCmdThing = $false
    $skipBlock = $false
    $blocksToSkip = "usd_AppliedDatabaseTestDataScript", "IX_usd_DateApplied"
    $noDiff = $true
    $noDiffLines = "", "GO", "PRINT N'Update complete.';" #these are the only lines left when there are no DB diffs
    foreach($line in $scriptLines)
    {   
        #don't add anything until we get past the line USE [`$(DatabaseName)]; -- all the previous stuff should be sqlcmd/unncessary junk 
        if ($line -eq "USE [`$(DatabaseName)];")
        {
            $passedLastSqlCmdThing = $true
        }
        #skip any blocks which contain any of the text in the skippable array
        elseif ($blocksToSkip | Where-Object { $line.Contains($_) })
        {
             $skipBlock = $true
        }        
        elseif ($passedLastSqlCmdThing -and -not $skipBlock)
        {
            $newLine = "$line"
            Add-Content $databaseScriptsUpdate\$newScriptName "$newLine"

            if ($noDiff)
            {
                $noDiff = $noDiffLines.Contains($newLine)
            }
        }
        elseif ($line -eq "GO")
        {
            $skipBlock = $false
        }
    }    

    write-host "Cleaning up temporary files and databases..."
    exec { & del $databaseScripts\$datebase_name.dacpac }
    exec { & sqlcmd -S .\SQLEXPRESS -Q "DROP DATABASE $databaseName_Original"  }

    if ($noDiff)
    {
        Remove-Item $databaseScriptsUpdate\$newScriptName
         write-host "No schema changes found for $datebase_name" -foregroundcolor "green"
    }
    else 
    {
        write-host "Please validate the new script $databaseScriptsUpdate\$newScriptName is correct, then rename to .sql and add to the database project" -foregroundcolor "yellow"
    }
}

function null-coalesce($a, $b) { if ($a -ne $null) { $a } else { $b } }
